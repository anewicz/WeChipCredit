using System;
using System.Collections.Generic;
using System.Linq;
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
                cbItem.Tag = item.Id;
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
                    cbItem.Tag = item.Id;
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
                    var clientId = (client.Id).ToString();
                    int clientId2 = int.Parse(clientId);

                    if (DeleteCheck.IsChecked == true)
                    {
                        var findClient = _Clients.Where(w => w.Id == clientId2).FirstOrDefault();
                        CliId.Text = client.Id.ToString();
                    }
                    else
                    {
                        var finder = _Clients.Where(w => w.Id == clientId2).FirstOrDefault();

                        CliName.Text = finder.Name;
                        CliCpf.Text = finder.Cpf.ToString();
                        CliCredit.Text = finder.VlCredit.ToString();
                        CliDdd.Text = finder.Ddd.ToString();
                        CliPhone.Text = finder.Phone.ToString();
                        CliId.Text = client.Id.ToString();

                        sttId.Text = finder._Status.Id.ToString();
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

            var msg = string.IsNullOrEmpty(CliName.Text) ? "NOME, " : "";
            msg += string.IsNullOrEmpty(CliCpf.Text) ? "CPF, " : "";
            msg += string.IsNullOrEmpty(CliDdd.Text) ? "DDD, " : "";
            msg += string.IsNullOrEmpty(CliPhone.Text) ? "TELEFONE, " : "";

            if (msg == "")
            {
                /*Padroniza o CPF e Retira tudo que não é Numérico do CPF e valida numeração restante se é CPF valido*/
                var CPF = string.Join("", CliCpf.Text.ToCharArray().Where(Char.IsDigit));
                var DDD = string.Join("", CliDdd.Text.ToCharArray().Where(Char.IsDigit));
                var Fone = string.Join("", CliPhone.Text.ToCharArray().Where(Char.IsDigit));

                if (!CPFValidate.ValidationCPF(CPF))
                    AtentionBox("Digite um CPF Válido com 11 Digitos Numericos", 2);
                else if (DDD.Length != 2)
                    AtentionBox("DDD Inválido - Deve Conter 2 Digitos Numericos", 2);
                else if (Fone.Length > 9 && Fone.Length < 8)
                    AtentionBox("Telefone Inválido - Telefone deve conter entre 8 e 9 Digitos Numericos", 2);
                else
                {
                    var ClienName = CliName.Text.ToUpper();
                    var ClienCpf = long.Parse(CPF);
                    var ClienDdd = sbyte.Parse(DDD);
                    var ClienPhone = int.Parse(Fone);
                    var ClienVlCredit = string.IsNullOrEmpty(CliCredit.Text) ? 0 : float.Parse(CliCredit.Text.Replace(".", ","));


                    var status = _Status.Where(w => w.Id == 1).FirstOrDefault();

                    Client client = new Client();
                    {
                        client.Id = _Clients.Count() + 1;
                        client.Name = ClienName;
                        client.Cpf = CPF;
                        client.Ddd = ClienDdd;
                        client.Phone = ClienPhone;
                        client.VlCredit = ClienVlCredit;
                        client._Status = status;
                    }
                    _Clients.Add(client);
                    ClientList.Items.Clear();
                    PopulateClientData();
                    ClientRefreshFields();

                    MessageBox.Show($" {client.Name} - Foi cadastrado com Sucesso!");

                }
            }
            else
                AtentionBox(msg, 1);
        }

        /*Editar Cliente*/
        private void Button_ClickEditRegister(object sender, RoutedEventArgs e)
        {
            var msg = string.IsNullOrEmpty(CliName.Text) ? "NOME, " : "";
            msg += string.IsNullOrEmpty(CliCpf.Text) ? "CPF, " : "";
            msg += string.IsNullOrEmpty(CliDdd.Text) ? "DDD, " : "";
            msg += string.IsNullOrEmpty(CliPhone.Text) ? "TELEFONE, " : "";

            if (string.IsNullOrEmpty(CliId.Text))
                AtentionBox("Selecione um cliente para Editar!", 2);
            else if (msg == "")
            {
                /*Padroniza o CPF e Retira tudo que não é Numérico do CPF e valida numeração restante se é CPF valido*/
                var CPF = string.Join("", CliCpf.Text.ToCharArray().Where(Char.IsDigit));
                var DDD = string.Join("", CliDdd.Text.ToCharArray().Where(Char.IsDigit));
                var Fone = string.Join("", CliPhone.Text.ToCharArray().Where(Char.IsDigit));

                if (!CPFValidate.ValidationCPF(CPF))
                    AtentionBox("Digite um CPF Válido com 11 Digitos Numericos", 2);
                else if (DDD.Length != 2)
                    AtentionBox("DDD Inválido - Deve Conter 2 Digitos Numericos", 2);
                else if (Fone.Length > 9 && Fone.Length < 8)
                    AtentionBox("Telefone Inválido - Telefone deve conter entre 8 e 9 Digitos Numericos", 2);
                else
                {
                    var clientId = int.Parse(CliId.Text);
                    var clientID = _Clients.Where(w => w.Id == clientId).FirstOrDefault();

                    if (MessageBox.Show($@"Deseja mesmo Alterar {clientID.Name.ToUpper()}?", "Confirmação", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        clientID.Name = CliName.Text.ToUpper();
                        clientID.Cpf = CPF;
                        clientID.Ddd = sbyte.Parse(DDD);
                        clientID.Phone = int.Parse(Fone);
                        clientID.VlCredit = float.Parse(CliCredit.Text);
                        ClientList.Items.Clear();
                        PopulateClientData();
                        ClientRefreshFields();

                        MessageBox.Show($@" {clientID.Name.ToUpper()} foi alterado com Sucesso!");

                    }

                }
            }
            else
                AtentionBox(msg, 1);
        }

        /*Deletar Cliente*/
        private void Button_ClickDeleteRegister(object sender, RoutedEventArgs e)
        {
            try
            {
                var clientId = int.Parse(CliId.Text);

                var findClient = _Clients.Where(w => w.Id == clientId).FirstOrDefault();
                var clientNm = findClient.Name;


                if (MessageBox.Show($@"Deseja mesmo Deletar {clientNm.ToUpper()} ?", "Confirmação", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _Clients.Remove(findClient);
                    MessageBox.Show($@"{clientNm} Foi deletado com Sucesso!");
                    ClientList.Items.Clear();
                    PopulateClientData();
                    ClientRefreshFields();
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
                CollapsedStacksInMain();
                /*STACKS - Novo Cadastro*/
                CliCredit.IsEnabled = true;
                RegisterEditForms.Visibility = Visibility.Visible; /*Forms info Cliente*/
                ButtonStRegister.Visibility = Visibility.Visible;  /*Botao cadastrar novo*/
                SubMnMailing.Visibility = Visibility.Visible;

            }
            else if (EditCheck.IsChecked == true)
            {
                CollapsedStacksInMain();
                /*STACKS - Editar Cadastro*/
                RegisterEditForms.Visibility = Visibility.Visible; /*Forms info Cliente*/
                ButtonStEdit.Visibility = Visibility.Visible;      /*Botao editar*/
                ListRegister.Visibility = Visibility.Visible;      /*Lista para editar*/
                CliStId.Visibility = Visibility.Visible;           /*Campo ID e Status*/
                SubMnMailing.Visibility = Visibility.Visible;

            }
            else if (DeleteCheck.IsChecked == true)
            {
                CollapsedStacksInMain();
                /*STACKS - Deletar Cadastro*/
                ListRegister.Visibility = Visibility.Visible;
                ButtonStDelete.Visibility = Visibility.Visible;
                SubMnMailing.Visibility = Visibility.Visible;
            };

        }

        #endregion Botoes

        #endregion TelaCadastro

        #region MetodosUteis

        /*Atualiza campos text*/
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

        }

        #endregion MetodosUteis


        #region TelaOfertar
        private void Button_ClickOfertar(object sender, RoutedEventArgs e)
        {
            try
            {
                var msg = string.IsNullOrEmpty(OfferName.Text) ? "NOME, " : "";
                msg += string.IsNullOrEmpty(OfferDdd.Text) ? "DDD, " : "";
                msg += string.IsNullOrEmpty(OfferPhone.Text) ? "TELEFONE, " : "";

                var DDD = string.Join("", OfferDdd.Text.ToCharArray().Where(Char.IsDigit));
                var Fone = string.Join("", OfferPhone.Text.ToCharArray().Where(Char.IsDigit));

                var clientId = (int)((ComboBoxItem)SearchClients.SelectedItem).Tag;
                var cInfo = _Clients.Where(w => w.Id == clientId).FirstOrDefault();

                if (SearchStatus.SelectedItem == null)

                    MessageBox.Show($"Selecione um Status para continuar!");
                else
                {
                    var sttId = (int)((ComboBoxItem)SearchStatus.SelectedItem).Tag;
                    var selStatus = _Status.Where(w => w.Id == sttId).FirstOrDefault();

                    var offerClient = _Offers.Where(w => w._Client.Id == clientId).FirstOrDefault();

                    if (selStatus.IsSale == true && offerClient._Products.Count == 0)
                        MessageBox.Show($"Não é possivel efetuar venda sem o Produto!");
                    else if (selStatus.IsSale == true && offerClient.TotalOffer > offerClient._Client.VlCredit)
                        MessageBox.Show($"Saldo CRÉDITOS insuficientes!");
                    else if (selStatus.IsSale == false && offerClient._Products.Count > 0)
                        MessageBox.Show($"Não é possivel RECUSAR com Produtos no carrinho!");
                    else
                    {
                        offerClient._Client._Status = selStatus;

                        cInfo.Name = OfferName.Text;
                        cInfo.Ddd = sbyte.Parse(OfferDdd.Text);
                        cInfo.Phone = int.Parse(OfferPhone.Text);

                        ClientList.Items.Clear();
                        PopulateClientData();
                        ClientRefreshFields();

                        MessageBox.Show($"Oferta salva com SUCESSO!");
                        PopulateOfferProductsData();

                        //if (MessageBox.Show($@"Deseja mesmo Alterar {clientID.Name.ToUpper()}?", "Confirmação", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        //{
                        //    clientID.Name = CliName.Text.ToUpper();
                        //    clientID.Cpf = CPF;
                        //    clientID.Ddd = sbyte.Parse(DDD);
                        //    clientID.Phone = int.Parse(Fone);
                        //    clientID.VlCredit = float.Parse(CliCredit.Text);
                        //    ClientList.Items.Clear();
                        //    PopulateClientData();
                        //    ClientRefreshFields();

                        //    MessageBox.Show($@" {clientID.Name.ToUpper()} foi alterado com Sucesso!");

                        //}
                    }
                }
            }
            catch { MessageBox.Show($"Por favor preencha todos os itens da oferta!"); }


        }

        #region Botoes

        /*Adiciona Produto a Oferta*/
        private void Button_ProductAdd(object sender, RoutedEventArgs e)
        {
            try
            {
                var clientId = (int)((ComboBoxItem)SearchClients.SelectedItem).Tag;
                var cInfo = _Clients.Where(w => w.Id == clientId).FirstOrDefault();

                var productId = (int)((ComboBoxItem)SearchProducts.SelectedItem).Tag;
                var _auxProd = _Products.Where(w => w.Id == productId).FirstOrDefault();

                var offerClient = _Offers.Where(w => w._Client.Id == clientId).FirstOrDefault();

                _OfferProducts.Add(_auxProd);
                offerClient._Products.Add(_auxProd);
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
                var cInfo = _Clients.Where(w => w.Id == clientId).FirstOrDefault();

                var product = (Product)OffersProductsList.SelectedItems[0];

                var offerClient = _Offers.Where(w => w._Client.Id == clientId).FirstOrDefault();

                _OfferProducts.Remove(product);
                offerClient._Products.Remove(product);
                PopulateOfferProductsData();
            }
            catch
            {
                MessageBox.Show($"O Produto não foi selecionado");
            }

        }

        private void OffersProductsList_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = (ListView)sender;
            var product = (Product)item.SelectedItem;
            if (product != null)
            {
                var idProduct = int.Parse(product.Id.ToString());
            }
        }

        /*Consulta Clientes Base*/
        private void Button_Search(object sender, RoutedEventArgs e)
        {
            var name = SeaNameCpf.Text;
            CollapsedStacksInMain();
            SearchClients.Items.Clear();
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
                        item.Content = $" CPF: {s.Cpf} | NOME: {s.Name}";
                        item.Tag = s.Id;
                        SearchClients.Items.Add(item);
                    }

                    if (resultSearch.Count() > 0)
                    {
                        SearchOfferInfos.Visibility = Visibility.Visible;
                        SeaNameCpf.Clear();
                    }
                    else
                    {
                        SearchOfferInfos.Visibility = Visibility.Collapsed;
                        MessageBox.Show($"O Cliente não foi encontrado!");
                        SeaNameCpf.Clear();
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
        /*Lista clientes dentro da regra cria oferta*/
        private void SearchClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (SearchClients.SelectedItem != null)
            {
                var clientId = (int)((ComboBoxItem)SearchClients.SelectedItem).Tag;
                var cInfo = _Clients.Where(w => w.Id == clientId).FirstOrDefault();

                PopulateProductData();
                conteudo.Document.Blocks.Clear();

                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add(new Bold(new Run("CÓD CLIENTE: ")));
                paragraph.Inlines.Add(new Run($"{cInfo.Id:0000}" + Environment.NewLine));
                paragraph.Inlines.Add(new Bold(new Run("CRÉDITO: ")));
                paragraph.Inlines.Add(new Run($" R${cInfo.VlCredit:0.00}" + Environment.NewLine));
                paragraph.Inlines.Add(new Bold(new Run("STATUS ATUAL: ")));
                paragraph.Inlines.Add(new Run($"{cInfo._Status.NmStatus}"));
                conteudo.Document = new FlowDocument(paragraph);

                Offer offer = new Offer();
                {
                    offer.Id = _Offers.Count() + 1;
                    offer._Client = cInfo;
                    offer.TotalOffer = 0;
                    offer._Products = new List<Product>();
                }

                OfferName.Text = cInfo.Name;
                OfferDdd.Text = cInfo.Ddd.ToString();
                OfferPhone.Text = cInfo.Phone.ToString();

                _Offers.Add(offer);
                ClientList.Items.Clear();
                PopulateClientData();
                ClientRefreshFields();
            }
            else
            {
                ClientList.Items.Clear();
                conteudo.Document.Blocks.Clear();
                OfferName.Clear();
                OfferDdd.Clear();
                OfferPhone.Clear();
                SeaNameCpf.Clear();
                PopulateClientData();
                ClientRefreshFields();
            }

        }

        /*Lista Status e atualiza a Oferta*/
        private void SearchStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (SearchClients.SelectedItem != null)
            {
                var clientId = (int)((ComboBoxItem)SearchClients.SelectedItem).Tag;
                var cInfo = _Clients.Where(w => w.Id == clientId).FirstOrDefault();

                if (SearchStatus.SelectedItem != null)
                {
                    var statusId = (int)((ComboBoxItem)SearchStatus.SelectedItem).Tag;
                    var sInfo = _Status.Where(w => w.Id == statusId).FirstOrDefault();

                    var offerClient = _Offers.Where(w => w._Client.Id == clientId).FirstOrDefault();

                    offerClient._Client._Status.Id = sInfo.Id;
                    offerClient._Client._Status.NmStatus = sInfo.NmStatus;
                    offerClient._Client._Status.CodStatus = sInfo.CodStatus;
                    offerClient._Client._Status.IsFinalizingClient = sInfo.IsFinalizingClient;
                    offerClient._Client._Status.IsSale = sInfo.IsSale;
                }
            }

        }

        #endregion ListasSuspensas

        #endregion TelaOfertar


    }
}

