using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using Windows.Networking.Sockets;
using CardDataBase;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Microsoft.Phone.Tasks;
using Windows.Networking.Proximity;
using CardCollector.Resources;

namespace CardCollector
{
    public partial class SwapCard : PhoneApplicationPage
    {
        Cards card;

        ObservableCollection<PeerAppInfo> _peerApps;
        StreamSocket _socket;
        string _peerName = string.Empty;

        // Error code constants
        const uint ERR_BLUETOOTH_OFF = 0x8007048F;      // The Bluetooth radio is off
        const uint ERR_MISSING_CAPS = 0x80070005;       // A capability is missing from your WMAppManifest.xml
        const uint ERR_NOT_ADVERTISING = 0x8000000E;    // You are currently not advertising your presence using PeerFinder.Start()

        public SwapCard()
        {
            InitializeComponent();

            SystemTray.SetProgressIndicator(this, new ProgressIndicator());
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Maintain a list of peers and bind that list to the UI
            _peerApps = new ObservableCollection<PeerAppInfo>();
            PeerList.ItemsSource = _peerApps;

            // Register for incoming connection requests
            PeerFinder.ConnectionRequested += PeerFinder_ConnectionRequested;

            // Start advertising ourselves so that our peers can find us
            PeerFinder.Start();

            RefreshPeerAppList();

            base.OnNavigatedTo(e);

            string parameterValue = NavigationContext.QueryString["parameter"];
            
            card = new Cards();
            card = card.getCard(int.Parse(parameterValue));
            this.DataContext = card;
        }

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            PeerFinder.ConnectionRequested -= PeerFinder_ConnectionRequested;

            // Cleanup before we leave
            CloseConnection(false);

            base.OnNavigatingFrom(e);
        }

        void PeerFinder_ConnectionRequested(object sender, ConnectionRequestedEventArgs args)
        {
            try
            {
                this.Dispatcher.BeginInvoke(() =>
                {
                    // Ask the user if they want to accept the incoming request.
                    var result = MessageBox.Show(String.Format("Deseja trocar Cards?", args.PeerInformation.DisplayName)
                                                 , "Requisição de troca!", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.OK)
                    {
                        ConnectToPeer(args.PeerInformation);
                    }
                    else
                    {
                        // Currently no method to tell the sender that the connection was rejected.
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                CloseConnection(true);
            }
        }

        async void ConnectToPeer(PeerInformation peer)
        {
            try
            {
                _socket = await PeerFinder.ConnectAsync(peer);

                // We can preserve battery by not advertising our presence.
                PeerFinder.Stop();

                _peerName = peer.DisplayName;
                UpdateCard("Troca Iniciada", true);

                // Since this is a chat, messages can be incoming and outgoing. 
                // Listen for incoming messages.
                ListenForIncomingMessage();
            }
            catch (Exception ex)
            {
                // In this sample, we handle each exception by displaying it and
                // closing any outstanding connection. An exception can occur here if, for example, 
                // the connection was refused, the connection timeout etc.
                MessageBox.Show(ex.Message);
                CloseConnection(false);
            }
        }

        private DataReader _dataReader;
        private async void ListenForIncomingMessage()
        {
            try
            {
                var message = await GetMessage();

                // Add to chat
                UpdateCard(message, true);

                // Start listening for the next message.
                ListenForIncomingMessage();
            }
            catch (Exception)
            {
                UpdateCard("Troca Encerrada", true);
                CloseConnection(true);
            }
        }

        private void CloseConnection(bool continueAdvertise)
        {
            if (_dataReader != null)
            {
                _dataReader.Dispose();
                _dataReader = null;
            }

            if (_dataWriter != null)
            {
                _dataWriter.Dispose();
                _dataWriter = null;
            }

            if (_socket != null)
            {
                _socket.Dispose();
                _socket = null;
            }

            if (continueAdvertise)
            {
                // Since there is no connection, let's advertise ourselves again, so that peers can find us.
                PeerFinder.Start();
            }
            else
            {
                PeerFinder.Stop();
            }
        }

        private async Task<string> GetMessage()
        {
            if (_dataReader == null)
                _dataReader = new DataReader(_socket.InputStream);

            // Each message is sent in two blocks.
            // The first is the size of the message.
            // The second if the message itself.
            await _dataReader.LoadAsync(4);
            uint messageLen = (uint)_dataReader.ReadInt32();
            await _dataReader.LoadAsync(messageLen);
            return _dataReader.ReadString(messageLen);
        }

        /// <summary>
        /// Asynchronous call to re-populate the ListBox of peers.
        /// </summary>
        private async void RefreshPeerAppList()
        {
            try
            {
                StartProgress("Procurando Pessoas...");
                var peers = await PeerFinder.FindAllPeersAsync();

                // By clearing the backing data, we are effectively clearing the ListBox
                _peerApps.Clear();

                if (peers.Count == 0)
                {
                    tbPeerList.Text = "Ninguém encontrado para trocar cards.";
                }
                else
                {
                    tbPeerList.Text = String.Format("Foram encontradas pessoas para trocar!", peers.Count);
                    // Add peers to list
                    foreach (var peer in peers)
                    {
                        _peerApps.Add(new PeerAppInfo(peer));
                    }

                    // If there is only one peer, go ahead and select it and conect to it.
                    if (PeerList.Items.Count == 1)
                    {
                        PeerList.SelectedIndex = 0;
                        performConnection();
                    }
                }
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == ERR_BLUETOOTH_OFF)
                {
                    var result = MessageBox.Show("Bluetooth desligado, tente novamente", "Erro", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.OK)
                    {
                        ShowBluetoothControlPanel();
                    }
                }
                else if ((uint)ex.HResult == ERR_MISSING_CAPS)
                {
                    MessageBox.Show("Faltam permissões");
                }
                else if ((uint)ex.HResult == ERR_NOT_ADVERTISING)
                {
                    MessageBox.Show("Erro ao se apresentar para o outro aparelho");
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
            finally
            {
                StopProgress();
            }
        }

        DataWriter _dataWriter;
        private void SendMessage_Tap_1(object sender, GestureEventArgs e)
        {
            SendMessage(card.id.ToString());
        }

        private async void SendMessage(string message)
        {
            if (message.Trim().Length == 0)
            {
                MessageBox.Show("Nenhum card para trocar foi selecionado", "Erro ao selecionar card", MessageBoxButton.OK);
                return;
            }

            if (_socket == null)
            {
                MessageBox.Show("Não está conectado a ninguém", "Erro ao conectar", MessageBoxButton.OK);
                return;
            }

            if (_dataWriter == null)
                _dataWriter = new DataWriter(_socket.OutputStream);

            // Each message is sent in two blocks.
            // The first is the size of the message.
            // The second if the message itself.
            _dataWriter.WriteInt32(message.Length);
            await _dataWriter.StoreAsync();

            _dataWriter.WriteString(message);
            await _dataWriter.StoreAsync();

            UpdateCard(message, false);
        }

        //You've swapped your card with someone, so increase or decrease it depending if you sent or received it
        //Also, just for make sure, when the user connects to the another player we show a 
        private void UpdateCard(string message, bool isIncoming)
        {
            if (isIncoming)
            {
                if (message.Length == 1)
                {
                    Cards receivedCard = card.getCard(int.Parse(message));
                    receivedCard.Increase();

                    message = "Recebeu um card do jogador " + receivedCard.PlayerName + " do time " + receivedCard.PlayerTeam + " de " + _peerName;
                }
                else
                {
                     message = (String.IsNullOrEmpty(_peerName)) ? String.Format(AppResources.Format_IncomingMessageNoName, message) : String.Format(AppResources.Format_IncomingMessageWithName, _peerName, message);
                }
            }
            else
            {
                if (message.Length == 1)
                {
                    Cards sentCard = card.getCard(int.Parse(message));
                    sentCard.Decrease();

                    message = "Enviou um card do jogador " + sentCard.PlayerName + " do time " + sentCard.PlayerTeam + " de " + _peerName;
                }
                else
                {
                      message = String.Format(AppResources.Format_OutgoingMessage, message);
                }
            }

            this.Dispatcher.BeginInvoke(() =>
            {
                tbChat.Text = message + tbChat.Text;
            });

            //After swapping card, refresh the actual card
            card.getCard(card.id);
            this.DataContext = card;
        }

        private void StartProgress(string message)
        {
            SystemTray.ProgressIndicator.Text = message;
            SystemTray.ProgressIndicator.IsIndeterminate = true;
            SystemTray.ProgressIndicator.IsVisible = true;
        }

        private void StopProgress()
        {
            if (SystemTray.ProgressIndicator != null)
            {
                SystemTray.ProgressIndicator.IsVisible = false;
                SystemTray.ProgressIndicator.IsIndeterminate = false;
            }
        }

        private void ShowBluetoothControlPanel()
        {
            ConnectionSettingsTask connectionSettingsTask = new ConnectionSettingsTask();
            connectionSettingsTask.ConnectionSettingsType = ConnectionSettingsType.Bluetooth;
            connectionSettingsTask.Show();
        }

        private void btnSendMessage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SendMessage(card.id.ToString());
        }

        private void FindPeers_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            RefreshPeerAppList();
        }

        private void ConnectToSelected_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            performConnection();
        }

        private void performConnection()
        {
            if (PeerList.SelectedItem == null)
            {
                MessageBox.Show("Ninguém selecionado", "Erro", MessageBoxButton.OK);
                return;
            }

            // Connect to the selected peer.
            PeerAppInfo pdi = PeerList.SelectedItem as PeerAppInfo;
            PeerInformation peer = pdi.PeerInfo;

            ConnectToPeer(peer);
        }
    }

    public class PeerAppInfo
    {
        internal PeerAppInfo(PeerInformation peerInformation)
        {
            this.PeerInfo = peerInformation;
            this.DisplayName = this.PeerInfo.DisplayName;
        }

        public string DisplayName { get; private set; }
        public PeerInformation PeerInfo { get; private set; }
    }
}