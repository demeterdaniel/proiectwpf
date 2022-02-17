
using AutoLotModel;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using Microsoft.Win32;
using System.IO;

namespace wpfproiect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    enum ActionState
    {
        New,
        Edit,
        Delete,
        Nothing
    }
    public partial class MainWindow : Window
    {
        ActionState action = ActionState.Nothing;
        AutoLotEntitiesModel ctx = new AutoLotEntitiesModel();
        CollectionViewSource cartiVSource;
        CollectionViewSource autoriVSource;
        CollectionViewSource atVSource;


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            cartiVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("cartiViewSource")));
            cartiVSource.Source = ctx.Cartis.Local;
            ctx.Cartis.Load();

            autoriVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("autoriViewSource")));
            autoriVSource.Source = ctx.Autoris.Local;
            ctx.Autoris.Load();

            atVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("titluri_autoriViewSource")));
            atVSource.Source = ctx.Titluri_autori.Local;
            ctx.Titluri_autori.Load();


         
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            BindingOperations.ClearBinding(descriereTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(titluTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(numeTextBox, TextBox.TextProperty);
            SetValidationBinding();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            TabItem ti = tbCtrlAutoLot.SelectedItem as TabItem;

            switch (ti.Header)
            {
                case "Carti":
                    SaveCarti();
                    break;
                case "Autori":
                    SaveAutori();
                    break;
              
            }
            ReInitialize();
            ReInitialize1();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ReInitialize();
            ReInitialize1();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            BindingOperations.ClearBinding(descriereTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(titluTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(numeTextBox, TextBox.TextProperty);
        }
        private void SaveCarti()
        {
            Carti carti = null;
            if (action == ActionState.New)
            {
              
                {
                    //instantiem Customer entity
                    carti = new Carti()
                    {
                        
                        Descriere = descriereTextBox.Text.Trim(),
                        Img_name = img_nameTextBox.Text.Trim(),
                        Titlu = titluTextBox.Text.Trim(),
                        Img_data = File.ReadAllBytes(img_nameTextBox.Text)
                };
                    //adaugam entitatea nou creata in context
                    ctx.Cartis.Add(carti);
                    cartiVSource.View.Refresh();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                //using System.Data;
                
            }
            else
           if (action == ActionState.Edit)
            {
                try
                {
                    carti = (Carti)cartiDataGrid.SelectedItem;
                    carti.Descriere = descriereTextBox.Text.Trim();
                    carti.Titlu = titluTextBox.Text.Trim();
                    carti.Img_name = img_nameTextBox.Text.Trim();
                    carti.Img_data = File.ReadAllBytes(img_nameTextBox.Text);

                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    carti = (Carti)cartiDataGrid.SelectedItem;
                    ctx.Cartis.Remove(carti);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                cartiVSource.View.Refresh();
            }

        }
        private void SaveAutori()
        {
            Autori autori = null;
            if (action == ActionState.New)
            {

                {
                    //instantiem Customer entity
                    autori = new Autori()
                    {

                  
                        Nume=numeTextBox.Text.Trim()
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Autoris.Add(autori);
                    autoriVSource.View.Refresh();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                //using System.Data;

            }
            else
           if (action == ActionState.Edit)
            {
                try
                {
                    autori = (Autori)autoriDataGrid.SelectedItem;
                    autori.Nume = numeTextBox.Text.Trim();

                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    autori = (Autori)autoriDataGrid.SelectedItem;
                    ctx.Autoris.Remove(autori);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                cartiVSource.View.Refresh();
            }

        }

        private void gbOperations_Click(object sender, RoutedEventArgs e)
        {
            Button SelectedButton = (Button)e.OriginalSource;
            Panel panel = (Panel)SelectedButton.Parent;

            foreach (Button B in panel.Children.OfType<Button>())
            {
                if (B != SelectedButton)
                    B.IsEnabled = false;
            }
            gbActions.IsEnabled = true;
            gbActions_Copy.IsEnabled = true;
        }
        private void ReInitialize()
        {

            Panel panel = gbOperations.Content as Panel;
            foreach (Button B in panel.Children.OfType<Button>())
            {
                B.IsEnabled = true;
            }
            gbActions.IsEnabled = false;
            gbActions_Copy.IsEnabled = false;

        }
        private void ReInitialize1()
        {

            Panel panel = gbOperations_copy.Content as Panel;
            foreach (Button B in panel.Children.OfType<Button>())
            {
                B.IsEnabled = true;
            }
      
            gbActions_Copy.IsEnabled = false;

        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            AutoLotEntitiesModel ctx = new AutoLotEntitiesModel();

            Carti images = new Carti();

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.ShowDialog();


            img_nameTextBox.Text = openFileDialog1.FileName;

            ImageSource imageSource = new BitmapImage(new Uri(img_nameTextBox.Text));

            ImageViewer1.Source = imageSource;
        }

       

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            Carti images = new Carti();

            var result = (from t in ctx.Cartis

                          where t.Img_name == img_nameTextBox.Text

                          select t.Img_data).FirstOrDefault();

            Stream StreamObj = new MemoryStream(result);

            BitmapImage BitObj = new BitmapImage();

            BitObj.BeginInit();

            BitObj.StreamSource = StreamObj;

            BitObj.EndInit();

            this.ImageViewer1.Source = BitObj;
        }

        private void SetValidationBinding()
        {
            Binding descriereValidationBinding = new Binding();
            descriereValidationBinding.Source = cartiVSource;
            descriereValidationBinding.Path = new PropertyPath("Descriere");
            descriereValidationBinding.NotifyOnValidationError = true;
            descriereValidationBinding.Mode = BindingMode.TwoWay;
            descriereValidationBinding.UpdateSourceTrigger =
           UpdateSourceTrigger.PropertyChanged;
            //string required
            descriereValidationBinding.ValidationRules.Add(new StringNotEmpty());
            descriereTextBox.SetBinding(TextBox.TextProperty,
            descriereValidationBinding);

            Binding titluValidationBinding = new Binding();
            titluValidationBinding.Source = cartiVSource;
            titluValidationBinding.Path = new PropertyPath("Titlu");
            titluValidationBinding.NotifyOnValidationError = true;
            titluValidationBinding.Mode = BindingMode.TwoWay;
            titluValidationBinding.UpdateSourceTrigger =
           UpdateSourceTrigger.PropertyChanged;
            //string min length validator
            titluValidationBinding.ValidationRules.Add(new
           StringMinLengthValidator());
            titluTextBox.SetBinding(TextBox.TextProperty,
           titluValidationBinding); //setare binding nou

            Binding numeValidationBinding = new Binding();
            numeValidationBinding.Source = autoriVSource;
            numeValidationBinding.Path = new PropertyPath("Nume");
            numeValidationBinding.NotifyOnValidationError = true;
            numeValidationBinding.Mode = BindingMode.TwoWay;
            numeValidationBinding.UpdateSourceTrigger =
           UpdateSourceTrigger.PropertyChanged;
            //string required
            numeValidationBinding.ValidationRules.Add(new StringNotEmpty());
            numeTextBox.SetBinding(TextBox.TextProperty,
            numeValidationBinding);
        }

        private void cartiDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void titluri_autoriDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
    
}
