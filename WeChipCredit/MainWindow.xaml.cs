using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using WeChipCredit.Models;



namespace WeChipCredit
{

    public partial class MainWindow : Window
    {
        List<Client> _Clients = new List<Client>();
        List<Product> _Products = new List<Product>();
        List<Status> _Status = new List<Status>();
        List<Offer> _Offers = new List<Offer>();
        List<Product> _OfferProducts = new List<Product>();
        Offer offerProcess = new Offer();

        public MainWindow()
        {
            /*Insere Base Inicial de Clientes*/
            _Clients = Client.GetFakeClients();
            InitializeComponent();
            PopulateStatusData();
            PopulateProductData();
            PopulateClientData();
        }



        #region VisibilidadeSacksMenuGeral

        /*Metodos para Definir quais niveis Stacks habilitar no Menu Principal- Conforme escolha */
        private void Button_Mailing(object sender, RoutedEventArgs e)
        {
            CollapsedStacksInMain();
            SubMnMailing.Visibility = Visibility.Visible;
            Button_SubRegisterOk(sender, e);
        }


        private void Button_Offer(object sender, RoutedEventArgs e)
        {
            CollapsedStacksInMain();
            Offer.Visibility = Visibility.Visible;
            PopulateStatusData();
            PopulateProductData();
        }

        #endregion VisibilidadeSacksMenuGeral

        #region PopularAsLists
        public void PopulateProductData()
        {
            SearchProducts.Items.Clear();

            _Products = Product.GetProducts();
            foreach (var item in _Products)
            {
                ComboBoxItem cbItem = new ComboBoxItem();
                cbItem.Content = $" CÓD: {item.CodProduct.ToString("0000")} | R$ {item.VlPrice.ToString("0.00")} - {item.NmProduct}";
                cbItem.Tag = item.IdProduct;
                SearchProducts.Items.Add(cbItem);
            }
        }

        public void PopulateOfferProductsData()
        {
            OffersProductsList.Items.Clear();
            var offerProductsData = _OfferProducts.ToList();
            foreach (var item in offerProductsData)
            {
                OffersProductsList.Items.Add(item);
            }
        }

        public void PopulateStatusData()
        {
            SearchStatus.Items.Clear();
            _Status = Status.GetStatus();
            foreach (var item in _Status)
            {
                if (item.CodStatus != 1)
                {
                    ComboBoxItem cbItem = new ComboBoxItem();
                    cbItem.Content = $"{item.CodStatus.ToString("0000")} - {item.NmStatus}";
                    cbItem.Tag = item.IdStatus;
                    SearchStatus.Items.Add(cbItem);

                }
            }

        }

        private void PopulateClientData()
        {
            var clientData = _Clients.ToList();
            foreach (var item in clientData)
            {
                ClientList.Items.Add(item);
            }
        }

        #endregion PopulaAsLists

        #region TelaCadastro

        #region ListView
        /*Seleciona registro e enviar ao Text Form Editar-Deletar */
        private void ClientList_PreviewMouseLeftBtUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var item = (ListView)sender;
                var client = (Client)item.SelectedItem;

                if (client != null)
                {
                    var clientId = (client.IdClient).ToString();
                    int clientId2 = int.Parse(clientId);

                    if (DeleteCheck.IsChecked == true)
                    {
                        var findClient = _Clients.Where(w => w.IdClient == clientId2).FirstOrDefault();
                        CliId.Text = client.IdClient.ToString();
                    }
                    else
                    {
                        var finder = _Clients.Where(w => w.IdClient == clientId2).FirstOrDefault();

                        CliName.Text = finder.Name;
                        CliCpf.Text = finder.Cpf.ToString();
                        CliCredit.Text = finder.VlCredit.ToString();
                        CliDdd.Text = finder.Ddd.ToString();
                        CliPhone.Text = finder.Phone.ToString();
                        CliId.Text = client.IdClient.ToString();

                        sttId.Text = finder._Status.IdStatus.ToString();
                        sttCode.Text = finder._Status.CodStatus.ToString();
                        sttName.Text = finder._Status.NmStatus.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Erro com sua Lista de Clientes!" + ex);
            }

        }
        #endregion ListView

        #region Botoes

        /*Adicionar Cliente*/
        private void Button_ClickNewRegister(object sender, RoutedEventArgs e)
        {
            bool resFieldValidate = valField("cliCad", 1);
            if (!resFieldValidate)
            {
                /*Padroniza o CPF e Retira tudo que não é Numérico do CPF e valida numeração restante se é CPF valido*/
                var CPF = string.Join("", CliCpf.Text.ToCharArray().Where(Char.IsDigit));
                var DDD = string.Join("", CliDdd.Text.ToCharArray().Where(Char.IsDigit));
                var Fone = string.Join("", CliPhone.Text.ToCharArray().Where(Char.IsDigit));

                float creditoParse;

                var result = float.TryParse(CliCredit.Text.Replace(".", ","), out creditoParse);
                if (!result)
                    AtentionBox("Credito precisa ser inserido no formato correto, somente numeração. Ex: 0,00", 2);
                else if (!CPFValidate.ValidationCPF(CPF))
                    AtentionBox("Digite um CPF Válido com 11 Digitos Numericos", 2);
                else if (DDD.Length != 2)
                    AtentionBox("DDD Inválido - Deve Conter 2 Digitos Numericos", 2);
                else if (Fone.Length > 9 || Fone.Length < 8)
                    AtentionBox("Telefone Inválido - Telefone deve conter entre 8 e 9 Digitos Numericos", 2);
                else
                {
 

                    var ClienName = CliName.Text.ToUpper();
                    var ClienCpf = long.Parse(CPF);
                    var ClienDdd = sbyte.Parse(DDD);
                    var ClienPhone = int.Parse(Fone);
                    var ClienVlCredit = string.IsNullOrEmpty(CliCredit.Text) ? 0 : creditoParse;

                    var status = _Status.Where(w => w.IdStatus == 1).FirstOrDefault();

                    var identMaxID = _Clients.Max(w => w.IdClient);

                    Client client = new Client();
                    {
                        client.IdClient = identMaxID + 1;
                        client.Name = ClienName;
                        client.Cpf = CPF;
                        client.Ddd = ClienDdd;
                        client.Phone = ClienPhone;
                        client.VlCredit = ClienVlCredit;
                        client._Status = status;
                    }
                    _Clients.Add(client);
                    ClientRefreshFields();
                    OfferRefreshFields();

                    MessageBox.Show($" {client.Name} - Foi cadastrado com Sucesso!");

                }
            }
        }

        /*Editar Cliente*/
        private void Button_ClickEditRegister(object sender, RoutedEventArgs e)
        {
            bool resFieldValidate = valField("cliCad", 1);

            if (string.IsNullOrEmpty(CliId.Text))
                AtentionBox("Selecione um cliente para Editar!", 2);
            else if (!resFieldValidate)
            {
                /*Padroniza o CPF e Retira tudo que não é Numérico do CPF e valida numeração restante se é CPF valido*/
                var CPF = string.Join("", CliCpf.Text.ToCharArray().Where(Char.IsDigit));
                var DDD = string.Join("", CliDdd.Text.ToCharArray().Where(Char.IsDigit));
                var Fone = string.Join("", CliPhone.Text.ToCharArray().Where(Char.IsDigit));

                if (!CPFValidate.ValidationCPF(CPF))
                    AtentionBox("Digite um CPF Válido com 11 Digitos Numericos", 2);
                else if (DDD.Length != 2)
                    AtentionBox("DDD Inválido - Deve Conter 2 Digitos Numericos", 2);
                else if (Fone.Length > 9 || Fone.Length < 8)
                    AtentionBox("Telefone Inválido - Telefone deve conter entre 8 e 9 Digitos Numericos", 2);
                else
                {
                    var clientId = int.Parse(CliId.Text);
                    var clientID = _Clients.Where(w => w.IdClient == clientId).FirstOrDefault();

                    if (MessageBox.Show($@"Deseja mesmo Alterar {clientID.Name.ToUpper()}?", "Confirmação", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        clientID.Name = CliName.Text.ToUpper();
                        clientID.Cpf = CPF;
                        clientID.Ddd = sbyte.Parse(DDD);
                        clientID.Phone = int.Parse(Fone);
                        clientID.VlCredit = float.Parse(CliCredit.Text);
                        ClientRefreshFields();
                        OfferRefreshFields();

                        MessageBox.Show($@" {clientID.Name.ToUpper()} foi alterado com Sucesso!");

                    }

                }
            }

        }

        /*Deletar Cliente*/
        private void Button_ClickDeleteRegister(object sender, RoutedEventArgs e)
        {
            try
            {
                var clientId = int.Parse(CliId.Text);

                var findClient = _Clients.Where(w => w.IdClient == clientId).FirstOrDefault();
                var clientNm = findClient.Name;


                if (MessageBox.Show($@"Deseja mesmo Deletar {clientNm.ToUpper()} ?", "Confirmação", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _Clients.Remove(findClient);
                    MessageBox.Show($@"{clientNm} Foi deletado com Sucesso!");
                    ClientRefreshFields();
                    OfferRefreshFields();
                }

            }
            catch
            {
                AtentionBox("Selecione o cliente que deseja deletar!", 2);
            }
        }

        /*Controlador de STACKS - Submenu Cadastro*/
        private void Button_SubRegisterOk(object sender, RoutedEventArgs e)
        {
            if (RegisterCheck.IsChecked == true)
            {
                cliTitle.Document.Blocks.Clear();
                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add(new Bold(new Run("CADASTRAR CLIENTE")));
                cliTitle.Document = new FlowDocument(paragraph);

                CollapsedStacksInMain();
                /*STACKS - Novo Cadastro*/
                CliCredit.IsEnabled = true;
                RegisterEditForms.Visibility = Visibility.Visible; /*Forms info Cliente*/
                ButtonStRegister.Visibility = Visibility.Visible;  /*Botao cadastrar novo*/
                SubMnMailing.Visibility = Visibility.Visible;
                clititleStack.Visibility = Visibility.Visible;

            }
            else if (EditCheck.IsChecked == true)
            {
                cliTitle.Document.Blocks.Clear();
                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add(new Bold(new Run("EDITAR CADASTRO ")));
                cliTitle.Document = new FlowDocument(paragraph);

                CollapsedStacksInMain();
                /*STACKS - Editar Cadastro*/
                RegisterEditForms.Visibility = Visibility.Visible; /*Forms info Cliente*/
                ButtonStEdit.Visibility = Visibility.Visible;      /*Botao editar*/
                ListRegister.Visibility = Visibility.Visible;      /*Lista para editar*/
                CliStId.Visibility = Visibility.Visible;           /*Campo ID e Status*/
                SubMnMailing.Visibility = Visibility.Visible;
                clititleStack.Visibility = Visibility.Visible;

            }
            else if (DeleteCheck.IsChecked == true)
            {
                cliTitle.Document.Blocks.Clear();
                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add(new Bold(new Run("DELETAR CADASTRO")));
                cliTitle.Document = new FlowDocument(paragraph);

                CollapsedStacksInMain();
                /*STACKS - Deletar Cadastro*/
                ListRegister.Visibility = Visibility.Visible;
                ButtonStDelete.Visibility = Visibility.Visible;
                SubMnMailing.Visibility = Visibility.Visible;
                clititleStack.Visibility = Visibility.Visible;
            };

        }

        #endregion Botoes

        #endregion TelaCadastro

        #region MetodosUteis

        /*limpa text dos clientes*/
        private void ClientRefreshFields()
        {
            CliName.Text = "";
            CliCpf.Text = "";
            CliCredit.Text = "";
            CliDdd.Text = "";
            CliPhone.Text = "";
            CliId.Text = "";

            sttId.Text = "";
            sttCode.Text = "";
            sttName.Text = "";

            ClientList.Items.Clear();
            PopulateClientData();

        }

        /*limpa text da Oferta*/
        private void OfferRefreshFields()
        {
            OfferZip.Text = "";
            OfferStreet.Text = "";
            OfferNumber.Text = "";
            OfferComplement.Text = "";
            OfferNeighborhood.Text = "";
            OfferState.Text = "";
            OfferCity.Text = "";

            OfferName.Text = "";
            OfferDdd.Text = "";
            OfferPhone.Text = "";

            SaldoRestante.Document.Blocks.Clear();
            SearchClients.Items.Clear();
            _OfferProducts.RemoveAll(w => w.IdProduct > 0);
            SearchProducts.Items.Clear();
            SearchStatus.Items.Clear();
            SeaNameCpf.Clear();

            PopulateStatusData();
            PopulateProductData();
        }

        /*Validação preenchimento*/
        private bool valField(string valTipe, int tipe)
        {
            /*tipe = 1 mensagem padrão obrigatoriedade, 2 = mensagem personalizada*/
            var msg = string.Empty;

            if (valTipe == "hardware")
            {
                msg = string.IsNullOrEmpty(OfferZip.Text) ? "CEP, " : "";
                msg += string.IsNullOrEmpty(OfferStreet.Text) ? "RUA, " : "";
                msg += string.IsNullOrEmpty(OfferNumber.Text) ? "Nº, " : "";
                msg += string.IsNullOrEmpty(OfferNeighborhood.Text) ? "BAIRRO, " : "";
                msg += string.IsNullOrEmpty(OfferState.Text) ? "ESTADO, " : "";
                msg += string.IsNullOrEmpty(OfferCity.Text) ? "CIDADE, " : "";

            }
            else if (valTipe == "cliOffert")
            {
                msg = string.IsNullOrEmpty(OfferName.Text) ? "NOME, " : "";
                msg += string.IsNullOrEmpty(OfferDdd.Text) ? "DDD, " : "";
                msg += string.IsNullOrEmpty(OfferPhone.Text) ? "TELEFONE, " : "";

            }
            else if (valTipe == "cliFoneInfo")
            {
                var DDD = string.Join("", OfferDdd.Text.ToCharArray().Where(Char.IsDigit));
                var Fone = string.Join("", OfferPhone.Text.ToCharArray().Where(Char.IsDigit));

                if (DDD.Length != 2)
                    msg = "DDD Inválido - Deve Conter 2 Digitos Numericos";
                else if (Fone.Length > 9 || Fone.Length < 8)
                    msg = "Telefone Inválido - Telefone deve conter entre 8 e 9 Digitos Numericos";
            }
            else if (valTipe == "cliCad")
            {
                msg = string.IsNullOrEmpty(CliName.Text) ? "NOME, " : "";
                msg += string.IsNullOrEmpty(CliCpf.Text) ? "CPF, " : "";
                msg += string.IsNullOrEmpty(CliDdd.Text) ? "DDD, " : "";
                msg += string.IsNullOrEmpty(CliPhone.Text) ? "TELEFONE, " : "";

            }

            if (msg != "")
            {
                AtentionBox(msg, tipe);
                return true;
            }
            else
                return false;
        }

        /*Simplificar o codigo para box de validação*/
        private static void AtentionBox(string text, int action)
        {
            if (action == 1)
                MessageBox.Show($"Atenção! O preenchimento do campo {text}é Obrigatório!");
            else
                MessageBox.Show($"Atenção! {text}");
        }

        /*Desabilitar todas as Stacks*/
        public void CollapsedStacksInMain()
        {
            CliCredit.IsEnabled = false;
            RegisterEditForms.Visibility = Visibility.Collapsed; /*Forms info Cliente*/
            ButtonStRegister.Visibility = Visibility.Collapsed;  /*Botao - Novo Cliente*/
            ButtonStEdit.Visibility = Visibility.Collapsed;      /*Botao - Cadastro Editar*/
            ButtonStDelete.Visibility = Visibility.Collapsed;    /*Botao - Cadastro Deletar*/
            ListRegister.Visibility = Visibility.Collapsed;      /*Lista - Cadastro Editar-Excluir */
            CliStId.Visibility = Visibility.Collapsed;           /*ID - Cadastro Editar*/
            SubMnMailing.Visibility = Visibility.Collapsed;      /*Submenu Cadastro*/
            SubMnOffers.Visibility = Visibility.Collapsed;       /*Submenu Ofertas*/
            SearchOfferInfos.Visibility = Visibility.Collapsed;      /*Oferta*/
            Offer.Visibility = Visibility.Collapsed;
            clititleStack.Visibility = Visibility.Collapsed;
        }

        #endregion MetodosUteis

        #region TelaOfertar
        private void Button_ClickOfertar(object sender, RoutedEventArgs e)
        {
            bool resFieldValidate = false;
            try
            {
                if (SearchClients.SelectedItem == null)
                    MessageBox.Show($"Obrigatorio selecionar um cliente para continuar!");
                else
                {
                    /*Valida campos do cliente/offerta para prosseguir*/
                    resFieldValidate = valField("cliOffert", 1);
                    if (!resFieldValidate)
                    {
                        var clientId = (int)((ComboBoxItem)SearchClients.SelectedItem).Tag;
                        var selClient = _Clients.Where(w => w.IdClient == clientId).FirstOrDefault();

                        /*Valida DDD e Fone antes de continuar*/
                        resFieldValidate = valField("cliFoneInfo", 2);
                        if (!resFieldValidate)
                        {
                            if (SearchStatus.SelectedItem == null)
                                MessageBox.Show($"Selecione um Status para continuar!");
                            else
                            {
                                var sttId = (int)((ComboBoxItem)SearchStatus.SelectedItem).Tag;
                                var selStatus = _Status.Where(w => w.IdStatus == sttId).FirstOrDefault();
                                //var offerClient = _Offers.Where(w => w._Client.IdClient == clientId).FirstOrDefault();

                                if (selStatus.IsSale == true && offerProcess._Products.Count == 0)
                                    MessageBox.Show($"Adicione um produto para venda!");
                                else if (selStatus.IsSale == true && offerProcess.TotalOffer > offerProcess._Client.VlCredit)
                                    MessageBox.Show($"Saldo Créditos insuficientes!");
                                else if (selStatus.IsSale == false && offerProcess._Products.Count > 0)
                                    MessageBox.Show($"Não é possivel recusar oferta com produtos adicionados para venda!");
                                else
                                {

                                    /*Valida se possui produtos tipo Hardware*/
                                    var hasHardware = _OfferProducts.Where(w => w.TpProduct == "HARDWARE").Count();
                                    if (hasHardware > 0)
                                        resFieldValidate = valField("hardware", 1);

                                    if (!resFieldValidate)
                                    {
                                        if (MessageBox.Show($@"Deseja salvar as modificações para {OfferName.Text.ToUpper()}?", "Confirmação", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                        {
                                            offerProcess._Client._Status = selStatus;
                                            selClient._Status = selStatus;
                                            selClient.Name = OfferName.Text.ToUpper();
                                            selClient.Ddd = sbyte.Parse(OfferDdd.Text);
                                            selClient.Phone = int.Parse(OfferPhone.Text);
                                            selClient.VlCreditAvailable = offerProcess._Client.VlCredit - offerProcess.TotalOffer;

                                            /*Congelar a Classe Cliente para guardar Ofertas anteriores ex: caiu a ligação e depois venda e não alterar nos dois*/
                                            offerProcess._Client = (Client)offerProcess._Client.Clone();

                                            /*Adiciona Endereço caso tenha*/
                                            if (!string.IsNullOrEmpty(OfferState.Text) && !string.IsNullOrEmpty(OfferCity.Text) && !string.IsNullOrEmpty(OfferStreet.Text))
                                            {
                                                DeliveryAddress address = new DeliveryAddress();
                                                {
                                                    address.IdAdress = 1;
                                                    address.ZipCode = OfferZip.Text;
                                                    address.Street = OfferStreet.Text.ToUpper();
                                                    address.Number = OfferNumber.Text.ToUpper();
                                                    address.Complement = OfferComplement.Text.ToUpper();
                                                    address.Neighborhood = OfferNeighborhood.Text.ToUpper();
                                                    address.State = OfferState.Text.ToUpper();
                                                    address.City = OfferCity.Text.ToUpper();
                                                }

                                                offerProcess._Client._Address = address;
                                            }

                                            try
                                            {
                                                var ofertaLimpa = (Offer)offerProcess.Clone();

                                                /*Insere a Venda na Lista de Ofertas*/
                                                _Offers.Add(ofertaLimpa);

                                                MessageBox.Show($@" Registro atualizado com Sucesso!");
                                                ClientRefreshFields();
                                                OfferRefreshFields();
                                                PopulateOfferProductsData();
                                                CollapsedStacksInMain();
                                                Offer.Visibility = Visibility.Visible;

                                            }
                                            catch { MessageBox.Show($@" Ocorreu um Erro ao salvar!"); }


                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show($"Por favor revise os itens da sua oferta!");
            }


        }

        #region Botoes

        /*Adiciona Produto a Oferta*/
        private void Button_ProductAdd(object sender, RoutedEventArgs e)
        {
            try
            {
                var clientId = (int)((ComboBoxItem)SearchClients.SelectedItem).Tag;
                var cInfo = _Clients.Where(w => w.IdClient == clientId).FirstOrDefault();

                var productId = (int)((ComboBoxItem)SearchProducts.SelectedItem).Tag;
                var _auxProd = _Products.Where(w => w.IdProduct == productId).FirstOrDefault();

                _OfferProducts.Add(_auxProd);
                offerProcess._Products.Add(_auxProd);
                RefreshVlOffer(offerProcess.TotalOffer, offerProcess._Client.VlCredit);
                PopulateOfferProductsData();
            }
            catch
            {
                MessageBox.Show($"O Produto não foi selecionado");
            }
        }

        /*Remove Produto a Oferta*/
        private void Button_ProductRemove(object sender, RoutedEventArgs e)
        {
            try
            {

                var clientId = (int)((ComboBoxItem)SearchClients.SelectedItem).Tag;
                var cInfo = _Clients.Where(w => w.IdClient == clientId).FirstOrDefault();

                var product = (Product)OffersProductsList.SelectedItems[0];

                _OfferProducts.Remove(product);
                offerProcess._Products.Remove(product);
                RefreshVlOffer(offerProcess.TotalOffer, offerProcess._Client.VlCredit);
                PopulateOfferProductsData();
            }
            catch
            {
                MessageBox.Show($"O Produto não foi selecionado");
            }

        }

        /*Campo de Saldo restante*/
        private void RefreshVlOffer(float valueOffer, float valueCredit)
        {

            float value = valueCredit - valueOffer;

            SaldoRestante.Document.Blocks.Clear();

            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(new Bold(new Run("Saldo Restante: ")));
            paragraph.Inlines.Add(new Run($" R${value:0.00}"));
            SaldoRestante.Document = new FlowDocument(paragraph);
        }

        private void OffersProductsList_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = (ListView)sender;
            var product = (Product)item.SelectedItem;
            if (product != null)
            {
                var idProduct = int.Parse(product.IdProduct.ToString());
            }
        }

        /*Consulta Clientes Base*/
        private void Button_Search(object sender, RoutedEventArgs e)
        {
            var name = SeaNameCpf.Text;
            OfferRefreshFields();
            CollapsedStacksInMain();
            PopulateOfferProductsData();
            Offer.Visibility = Visibility.Visible;

            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    var cpf = string.Join("", name.ToCharArray().Where(char.IsDigit));

                    /*Retorna somente clientes não finalizados*/
                    List<Client> resultSearch;
                    if (!string.IsNullOrEmpty(cpf))
                        resultSearch = _Clients.Where(w => w._Status.IsFinalizingClient == false && w.Cpf == cpf).ToList();
                    else
                        resultSearch = _Clients.Where(w => w._Status.IsFinalizingClient == false && w.Name.Contains(name.ToUpper())).ToList();

                    foreach (var s in resultSearch)
                    {
                        ComboBoxItem item = new ComboBoxItem();
                        item.Content = $" ID: {s.IdClient:0000} | CPF: {s.Cpf} | NOME: {s.Name}";
                        item.Tag = s.IdClient;
                        SearchClients.Items.Add(item);
                    }

                    if (resultSearch.Count() > 0)
                    {
                        SearchOfferInfos.Visibility = Visibility.Visible;

                    }
                    else
                    {
                        SearchOfferInfos.Visibility = Visibility.Collapsed;
                        MessageBox.Show($"O Cliente não foi encontrado!");

                    }
                }
            }
            catch
            {
                MessageBox.Show($"O Cliente não foi encontrado!");
            }
        }

        #endregion Botoes

        #region ListasSuspensas
        /*Lista clientes dentro da regra e cria oferta*/
        private void SearchClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (SearchClients.SelectedItem != null)
            {
                var clientId = (int)((ComboBoxItem)SearchClients.SelectedItem).Tag;
                var selClient = _Clients.Where(w => w.IdClient == clientId).FirstOrDefault();

                PopulateProductData();
                conteudo.Document.Blocks.Clear();

                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add(new Bold(new Run("CÓD CLIENTE: ")));
                paragraph.Inlines.Add(new Run($"{selClient.IdClient:0000}" + Environment.NewLine));
                paragraph.Inlines.Add(new Bold(new Run("CRÉDITO: ")));
                paragraph.Inlines.Add(new Run($" R${selClient.VlCredit:0.00}" + Environment.NewLine));
                paragraph.Inlines.Add(new Bold(new Run("STATUS ATUAL: ")));
                paragraph.Inlines.Add(new Run($"{selClient._Status.NmStatus}"));
                conteudo.Document = new FlowDocument(paragraph);

                offerProcess.IdOffer = _Offers.Count() + 1;
                offerProcess._Client = selClient;
                offerProcess.TotalOffer = 0;
                offerProcess._Products = new List<Product>();

                OfferName.Text = selClient.Name.ToUpper();
                OfferDdd.Text = selClient.Ddd.ToString();
                OfferPhone.Text = selClient.Phone.ToString();

                ClientList.Items.Clear();
                PopulateClientData();

                RefreshVlOffer(offerProcess.TotalOffer, offerProcess._Client.VlCredit);
            }
            else
            {
                ClientList.Items.Clear();
                conteudo.Document.Blocks.Clear();

                PopulateClientData();
                OfferRefreshFields();
            }

        }

        #endregion ListasSuspensas

        #endregion TelaOfertar


    }

}

