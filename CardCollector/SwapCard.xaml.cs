﻿using System;
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
        ObservableCollection<PeerAppInfo> _peerApps; //Local copy of available peer apps
        StreamSocket _socket;                        //Socket used for comunication with another device
        String _peerName = string.Empty;             //Name of the peer who we are trying to comunicate

        // Error code constants
        const uint ERR_BLUETOOTH_OFF = 0x8007048F;      // The Bluetooth radio is off
        const uint ERR_MISSING_CAPS = 0x80070005;       // A capability is missing from your WMAppManifest.xml
        const uint ERR_NOT_ADVERTISING = 0x8000000E;    // You are currently not advertising your presence using PeerFinder.Start()

        Cards card;

        public SwapCard(int id)
        {
            InitializeComponent();

            SystemTray.SetProgressIndicator(this, new ProgressIndicator());

            Cards cards = new Cards();

            card = cards.getCard(id);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Gets a list of peers and bind them to the list on the view
            _peerApps = new ObservableCollection<PeerAppInfo>();
            PeerList.ItemsSource = _peerApps;

            //Wait for other peers find us
            PeerFinder.ConnectionRequested += PeerFinder_ConnectionRequested;
            PeerFinder.Start();

            RefreshPeerAppList();

            base.OnNavigatedTo(e);
        }

        void PeerFinder_ConnectionRequested(object sender, ConnectionRequestedEventArgs args)
        {
            try
            {
                this.Dispatcher.BeginInvoke(() =>
                {
                    // Ask the user if they want to accept the incoming request.
                    var result = MessageBox.Show(String.Format("Você deseja trocar cards?"),"Trocar card", MessageBoxButton.OKCancel);
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


        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {

            PeerFinder.ConnectionRequested -= PeerFinder_ConnectionRequested;

            // Cleanup before we leave
            CloseConnection(false);

            base.OnNavigatingFrom(e);
        }

        async void ConnectToPeer(PeerInformation peer)
        {
            try
            {
                _socket = await PeerFinder.ConnectAsync(peer);

                // We can preserve battery by not advertising our presence.
                PeerFinder.Stop();

                _peerName = peer.DisplayName;
                UpdateChatBox("Troca iniciada", true);

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
                UpdateChatBox(message, true);

                // Start listening for the next message.
                ListenForIncomingMessage();
            }
            catch (Exception)
            {
                UpdateChatBox("Troca encerrada", true);
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
            //var len = await GetMessageSize();
            await _dataReader.LoadAsync(4);
            uint messageLen = (uint)_dataReader.ReadInt32();
            await _dataReader.LoadAsync(messageLen);
            return _dataReader.ReadString(messageLen);
        }

        private void FindPeers_Tap(object sender, GestureEventArgs e)
        {
            RefreshPeerAppList();
        }

        private async void RefreshPeerAppList()
        {
            try
            {
                StartProgress("finding peers ...");
                var peers = await PeerFinder.FindAllPeersAsync();

                // By clearing the backing data, we are effectively clearing the ListBox
                _peerApps.Clear();

                if (peers.Count == 0)
                {
                    tbPeerList.Text = "Nenhum outro aparelho encontrado";
                }
                else
                {
                    tbPeerList.Text = String.Format("Encontrados" + peers.Count);
                    // Add peers to list
                    foreach (var peer in peers)
                    {
                        _peerApps.Add(new PeerAppInfo(peer));
                    }

                    // If there is only one peer, go ahead and select it
                    if (PeerList.Items.Count == 1)
                        PeerList.SelectedIndex = 0;

                }
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == ERR_BLUETOOTH_OFF)
                {
                    var result = MessageBox.Show("Bluetooth desligado", "Erro", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.OK)
                    {
                        ShowBluetoothControlPanel();
                    }
                }
                else if ((uint)ex.HResult == ERR_MISSING_CAPS)
                {
                    MessageBox.Show("Para executar este App você deve ter ativado ID_CAP_PROXIMITY");
                }
                else if ((uint)ex.HResult == ERR_NOT_ADVERTISING)
                {
                    MessageBox.Show("Erro no advertise");
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
                MessageBox.Show("Nenhum card escolhido", "Erro", MessageBoxButton.OK);
                return;
            }

            if (_socket == null)
            {
                MessageBox.Show("Erro", "Nenhum celular conectado", MessageBoxButton.OK);
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

            UpdateChatBox(message, false);
        }

        private void UpdateChatBox(string message, bool isIncoming)
        {
            if (isIncoming)
            {
                message = (String.IsNullOrEmpty(_peerName)) ? String.Format(AppResources.Format_IncomingMessageNoName, message) : String.Format(AppResources.Format_IncomingMessageWithName, _peerName, message);
            }
            else
            {
                message = String.Format(AppResources.Format_OutgoingMessage, message);
            }

            this.Dispatcher.BeginInvoke(() =>
            {
                tbChat.Text = message + tbChat.Text;
                txtMessage.Text = (isIncoming) ? txtMessage.Text : string.Empty;
            });
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