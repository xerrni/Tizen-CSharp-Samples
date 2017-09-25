/*
 *  Copyright (c) 2017 Samsung Electronics Co., Ltd All Rights Reserved
 *
 *  Contact: Ernest Borowski <e.borowski@partner.samsung.com>
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License
 *
 *
 * @file        Certificates.cs
 * @author      Ernest Borowski (e.borowski@partner.samsung.com)
 * @version     1.0
 * @brief       This file contains implementation of UI
 */

namespace SecureRepository.Tizen
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using global::Tizen;
    using global::Tizen.Security.SecureRepository;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    /// <summary>
    /// Class responsible for UI.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        /// <summary>
        /// Used for showing Managers aliases in UI.
        /// </summary>
        public List<ListViewGroup> Groups;

        /// <summary>
        /// Holds App width. It is used when checking if width has changed.
        /// </summary>
        private double width;

        /// <summary>
        /// Holds App height. It is used when checking if height has changed.
        /// </summary>
        private double height;

        /// <summary>
        /// Used for storing data in DataManager and for checking file paths.
        /// </summary>
        private Data data;

        /// <summary>
        /// Used for accessing KeyManager.
        /// </summary>
        private Keys keys;

        /// <summary>
        /// Used for accessing CertificateManager.
        /// </summary>
        private Certificates certs;

        /// <summary>
        /// Initializes a new instance of the MainPage class.
        /// </summary>
        public MainPage()
        {
            this.data = new Data();
            this.keys = new Keys();
            this.certs = new Certificates();
            this.InitializeComponent();

            listView.ItemsSource = new string[] { };
            var template = new DataTemplate(typeof(TextCell));
            template.SetBinding(TextCell.TextProperty, "Alias");
            listView.ItemTemplate = template;
            this.ListItems();
        }

        /// <summary>
        /// Handles display orientation change (horizontal, vertical).
        /// </summary>
        /// <param name="width">new App width.</param>
        /// <param name="height">new App height.</param>
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            
            // width or height has changed.
            if (width != this.width || height != this.height)
            {
                this.width = width;
                this.height = height;

                MainGrid.RowDefinitions.Clear();
                MainGrid.ColumnDefinitions.Clear();
                MainGrid.Children.Clear();

                optionsGrid.RowDefinitions.Clear();
                optionsGrid.ColumnDefinitions.Clear();
                optionsGrid.Children.Clear();

                // Wearable UI, it`s much simplified.
                // (Screen height may not equals screen width so it only checks pixel count).
                if (height < 500 && width < 500)
                {
                    this.MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1.8, GridUnitType.Star) });
                    this.MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.8, GridUnitType.Star) });
                    this.MainGrid.Children.Add(this.listView, 0, 0);
                    this.MainGrid.Children.Add(this.appInfo, 0, 1);
                    appInfo.HorizontalOptions = LayoutOptions.Center;
                    this.OnAddKeyClicked(null, null); // User can only see already added items.
                }
                else if (width > height)
                {
                    // Realigns UI for vertical mode.
                    MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.6, GridUnitType.Star) });
                    MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                    optionsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    optionsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    optionsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    optionsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    optionsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                    optionsGrid.Children.Add(this.appInfo, 0, 0);
                    optionsGrid.Children.Add(this.contentPrefix, 0, 1);
                    optionsGrid.Children.Add(this.btnAdd, 0, 2);
                    optionsGrid.Children.Add(this.btnRemove, 0, 3);
                    optionsGrid.Children.Add(this.btnEncrypt, 0, 4);

                    MainGrid.Children.Add(this.listGrid, 0, 0);
                    MainGrid.Children.Add(this.optionsGrid, 1, 0);

                    // fix Listview size on TV emulator.
                    if (Device.Idiom == TargetIdiom.TV)
                    {
                        listView.Scale = 1.05;
                    }
                }
                else
                {
                    // Realigns UI for horizontal mode.
                    labelGrid.RowDefinitions.Clear();
                    labelGrid.ColumnDefinitions.Clear();
                    labelGrid.Children.Clear();

                    gridBtn.RowDefinitions.Clear();
                    gridBtn.ColumnDefinitions.Clear();
                    gridBtn.Children.Clear();

                    MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1.6, GridUnitType.Star) });
                    MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                    optionsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.6, GridUnitType.Star) });
                    optionsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                    labelGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    labelGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                    gridBtn.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    gridBtn.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    gridBtn.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                    labelGrid.Children.Add(this.appInfo, 0, 0);
                    labelGrid.Children.Add(this.contentPrefix, 0, 1);

                    gridBtn.Children.Add(this.btnAdd, 0, 0);
                    gridBtn.Children.Add(this.btnRemove, 0, 1);
                    gridBtn.Children.Add(this.btnEncrypt, 0, 2);

                    optionsGrid.Children.Add(this.labelGrid, 0, 0);
                    optionsGrid.Children.Add(this.gridBtn, 1, 0);

                    MainGrid.Children.Add(this.listGrid, 0, 0);
                    MainGrid.Children.Add(this.optionsGrid, 0, 1);
                }

                this.GridBtnSizeChanged(null, null); // Changes Buttons and Labels size to fit in new UI.
            }
        }

        /// <summary>
        /// Validates certificates and adds them to CertificateManager.
        /// </summary>
        private void AddCertificates()
        {
            // Certificates used in this example are from tizen.org. They are valid from 18 May 2017 till 18 July 2018.
            // So when running this app change your date to fit between this dates, otherwise You will get error that certificate is not valid.
            string chain_path = Tizen.Program.Current.DirectoryInfo.SharedResource + "tizen_pem_chain.crt";
            string path = Tizen.Program.Current.DirectoryInfo.SharedResource + "tizen_pem.crt";
            byte[] b = Encoding.UTF8.GetBytes("TESRASDAS");

            if (Data.CheckPath(path) && Data.CheckPath(chain_path))
            {
                IEnumerable<Certificate> list = null;
                list = this.certs.GetCertificates(chain_path);

                // Saves first certificate.
                if (list != null)
                {
                    // Checks if Certificate chain is correct.
                    if (this.certs.IsTrusted(list.First(), list)) 
                    {
                        this.certs.Save("tizen.org_cert", path); // Saves single certificate.
                    }
                    else
                    {
                        appInfo.Text += "Certificate is NOT TRUSTED so app will not save it inside CertificateManager"; // Displays info that Certificate chain is incorrect.
                    }
                }
            }
        }

        /// <summary>
        /// Adds data (user defined strings) to DataManager.
        /// </summary>
        private void AddData()
        {
            string testData = "TEST12321312dsajdas";
            string anotherData = "new_data_with_utf8_string_ĄĘŹŻŁć_서울"; // Another data to show that you can use UTF8 encoded data.
            // data.RemoveAll(); //Use it to manually remove all data from DataManager.
            this.data.Save(Encoding.UTF8.GetBytes(testData), "my_data", null);
            this.data.Save(Encoding.UTF8.GetBytes(anotherData), "my_data", null); // Will not succeed, because data is already added to this alias.
            this.data.Save(Encoding.UTF8.GetBytes(anotherData), "my_data_2", null); // This will succeed, because there is no data under "my_data_2" alias.
        }

        /// <summary>
        /// Adds Aes / Rsa Keys to KeyManager.
        /// </summary>
        private void AddKeys()
        {
            // keys.RemoveAll(); // KeyManager has previously keys even after reboot / recompile etc.
            // In normal app we don`t use this Method.
            this.keys.AddAesKey("my_aes");
            this.keys.AddPrivateRsaKey("my_private_rsa");
            this.keys.AddPublicRsaKey("my_public_rsa");
            this.keys.CreateRsaKeyPair("private_auto_rsa", "public_auto_rsa");
        }

        /// <summary>
        /// Gets content of selected item from listView.
        /// </summary>
        private void GetItemContent()
        {
            string type = null, prefix = null;
            Item selectedItem = null;
            
            // Gets selected item.
            try
            {
                if (listView.SelectedItem != null)
                {
                    selectedItem = listView.SelectedItem as Item;
                }
                else
                {
                    return;
                }
            }
            catch 
            {
                // Error trying to get selected item from listVieW e.g. no item selected.
                return;
            }

            // Exits if no item is selected.
            if (selectedItem == null) 
            {
                return;
            }

            // Gets item Type and it`s content prefix.
            try
            {
                switch (selectedItem.Type)
                {
                    case AliasType.Certificate:
                        type = this.certs.GetType(selectedItem.Alias);
                        prefix = this.certs.GetContentPrefix(selectedItem.Alias);
                        break;
                    case AliasType.Data:
                        type = this.data.GetType(); // data type is always byte[], so there is no need to pass any arguments to it.
                        prefix = this.data.GetContentPrefix(selectedItem.Alias);
                        break;
                    case AliasType.Key:
                        type = this.keys.GetType(selectedItem.Alias);
                        prefix = this.keys.GetContentPrefix(selectedItem.Alias);
                        break;
                }
            }
            catch (Exception ex) 
            {
                // Unable to get Data,Certificate,Key related to that alias.
                Log.Error("SecureRepository_UI", "Unable to get item data or prefix");
                Log.Error("SecureRepository_UI", ex.Message);
            }

            appInfo.Text = "Item type: " + type;
            contentPrefix.Text = "Item prefix: " + prefix;
        }

        /// <summary>
        /// Adjusts margins of labels and buttons.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void GridBtnSizeChanged(object sender, EventArgs e) // Changes buttons size to fit current UI.
        {
            double margin, marginLeftRight;

            // Horizontal View.
            if (gridBtn.Children.Count > 0) 
            {
                margin = gridBtn.Height * 0.02;
                marginLeftRight = gridBtn.Width * 0.15;
            }
            else 
            {
                // Vertical Views.
                margin = optionsGrid.Height * 0.02;
                marginLeftRight = optionsGrid.Height * 0.15;
            }

            // Adjusts buttons margins
            btnAdd.Margin = new Thickness(marginLeftRight, margin, marginLeftRight, margin);
            btnRemove.Margin = new Thickness(marginLeftRight, margin, marginLeftRight, margin);
            btnEncrypt.Margin = new Thickness(marginLeftRight, margin, marginLeftRight, margin);

            margin = appInfo.Height * 0.02;
            marginLeftRight = appInfo.Width * 0.02;

            // Adjusts labels margins.
            appInfo.Margin = new Thickness(marginLeftRight, margin, marginLeftRight, margin);
            contentPrefix.Margin = new Thickness(marginLeftRight, margin, marginLeftRight, margin);
        }

        /// <summary>
        /// Adds available aliases from all Managers to listView, Pkcs12 not included (it`s not part of this tutorial).
        /// </summary>
        private void ListItems()
        {
            IEnumerable<string> dataAliases, keyAliases, certificateAliases;
            keyAliases = this.keys.GetAliasesShort(); // Gets aliases from KeyManager without App.Name prefix.
            dataAliases = this.data.GetAliasesShort(); // Gets aliases from DataManager without App.Name prefix.
            certificateAliases = this.certs.GetAliasesShort(); // Gets aliases from CertificateManager without App.Name prefix.
            List<ListViewGroup> groups = new List<ListViewGroup>();
            int goupCount = 0;
            if (dataAliases != null)
            {
                groups.Add(new ListViewGroup(AliasType.Data));

                // Adds aliases to the list.
                foreach (string alias in dataAliases)  
                {
                    groups[goupCount].Add(new Item(alias, AliasType.Data));
                }

                goupCount++;
            }

            if (keyAliases != null)
            {
                groups.Add(new ListViewGroup(AliasType.Key));

                // Adds aliases to the list.
                foreach (string alias in keyAliases) 
                {
                    groups[goupCount].Add(new Item(alias, AliasType.Key));
                }

                goupCount++;
            }

            if (certificateAliases != null)
            {
                groups.Add(new ListViewGroup(AliasType.Certificate));

                // Adds aliases to the list.
                foreach (string alias in certificateAliases) 
                {
                    groups[goupCount].Add(new Item(alias, AliasType.Certificate));
                }

                goupCount++;
            }

            this.Groups = groups;
            this.listView.ItemsSource = this.Groups;
        }

        /// <summary>
        /// Adds items to Managers.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void OnAddKeyClicked(object sender, EventArgs e)
        {
            this.AddCertificates(); // Adds certificates to CertificateManager.
            this.AddKeys(); // Adds Keys to KeyManager.
            this.AddData(); // Adds Data to DataManager.
            this.ListItems(); // Shows changes to User -- adds items to listView.
        }

        /// <summary>
        /// Encrypt, Decrypt data.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void OnEncKeyClicked(object sender, EventArgs e)
        {
            string textToEncrypt = "string1_ĄĘŹŻŁć_서울", encText;
            Cryptography c = new Cryptography();

            byte[] b = c.Encrypt(System.Text.Encoding.UTF8.GetBytes(textToEncrypt)); // Encrypys string.

            if (b != null)
            {
                encText = "Encrypted:" + Cryptography.ByteArrayToHexViaLookup32(b); // Converts byte[] to hexadecimal string for reading only, in real app just use byte[].
                string decText;
                byte[] decBin = c.Decrypt(b);
                if (decBin != null)
                {
                    decText = "Orginal text: " + textToEncrypt + "\nDecrypted:    " + System.Text.Encoding.UTF8.GetString(decBin); // Displays decrypted text.
                }
                else
                {
                    decText = "Unable To decrypt text";
                }

                contentPrefix.Text = decText;
            }
            else
            {
                encText = "Unable To encrypt text";
            }

            appInfo.Text = encText;
            this.ListItems();
        }

        /// <summary>
        /// Removes all items from Managers and cleans up UI.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void OnRemoveKeyClicked(object sender, EventArgs e)
        {
            this.keys.RemoveAll(); // Removes all keys from KeyManager.
            this.data.RemoveAll(); // Removes all data from DataManager.
            this.certs.RemoveAll(); // Removes all certyficates from CertificateManager.
            appInfo.Text = string.Empty;
            contentPrefix.Text = string.Empty;
            this.ListItems(); // Updates list (it will be empty).
        }
    }
}