using ConvenienceStore.Views.Admin.ProductWindow;
using System;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command.ProductCommand.AddNewProductCommand
{
    public class DetermineCountryCommand : ICommand
    {
        static string[] MapBarCode = new string[1000];

        public DetermineCountryCommand()
        {
            for (int i = 0; i < MapBarCode.Length; i++)
            {
                MapBarCode[i] = "Unknow";
            }
            // US
            for (int i = 0; i < 19; i++)
                MapBarCode[i] = "United States";
            for (int i = 30; i < 39; i++)
                MapBarCode[i] = "United States";
            for (int i = 60; i < 139; i++)
                MapBarCode[i] = "United States";
            for (int i = 300; i < 379; i++)
                MapBarCode[i] = "France";
            // 
            MapBarCode[380] = "Bulgaria";
            MapBarCode[383] = "Slovenia";
            MapBarCode[385] = "Croatia";
            MapBarCode[387] = "Bosnia-Herzegovina";
            // 
            for (int i = 400; i < 440; i++)
                MapBarCode[i] = "Germany";
            //
            for (int i = 450; i < 459; i++)
                MapBarCode[i] = "Japan";
            //
            for (int i = 460; i < 469; i++)
                MapBarCode[i] = "Russia";
            //
            MapBarCode[470] = "Kurdistan";
            MapBarCode[471] = "Taiwan";
            MapBarCode[474] = "Estonia";
            MapBarCode[475] = "Latvia";
            MapBarCode[476] = "Azerbaijan";
            MapBarCode[477] = "Lithuania";
            MapBarCode[478] = "Uzbekistan";
            MapBarCode[479] = "Sri Lanka";
            MapBarCode[480] = "Philippines";
            MapBarCode[481] = "Belarus";
            MapBarCode[482] = "Ukraine";
            MapBarCode[484] = "Moldova";
            MapBarCode[485] = "Armenia";
            MapBarCode[486] = "Georgia";
            MapBarCode[487] = "Kazakhstan";
            MapBarCode[489] = "Hong Kong";
            //
            for (int i = 490; i < 499; i++)
                MapBarCode[i] = "Japan";
            //
            for (int i = 500; i < 509; i++)
                MapBarCode[i] = "United Kingdom";
            //
            MapBarCode[520] = "Greece";
            MapBarCode[528] = "Lebanon";
            MapBarCode[529] = "Cyprus";
            MapBarCode[530] = "Albania";
            MapBarCode[531] = "FYR Macedonia";
            MapBarCode[535] = "Malta";
            MapBarCode[539] = "Ireland";
            //
            for (int i = 540; i < 549; i++)
                MapBarCode[i] = "Belgium & Luxembourg";
            //
            MapBarCode[560] = "Portugal";
            MapBarCode[569] = "Iceland";
            //
            for (int i = 570; i < 579; i++)
                MapBarCode[i] = "Denmark";
            //
            MapBarCode[590] = "Poland";
            MapBarCode[594] = "Romania";
            MapBarCode[599] = "Hungary";
            MapBarCode[600] = "South Africa";
            MapBarCode[601] = "South Africa";
            MapBarCode[603] = "Ghana";
            MapBarCode[608] = "Bahrain";
            MapBarCode[609] = "Mauritius";
            MapBarCode[611] = "Morocco";
            MapBarCode[613] = "Algeria";
            MapBarCode[616] = "Kenya";
            MapBarCode[618] = "Ivory Coast";
            MapBarCode[619] = "Tunisia";
            MapBarCode[621] = "Syria";
            MapBarCode[622] = "Egypt";
            MapBarCode[624] = "Libya";
            MapBarCode[625] = "Jordan";
            MapBarCode[626] = "Iran";
            MapBarCode[627] = "Kuwait";
            MapBarCode[628] = "Saudi Arabia";
            MapBarCode[629] = "Emirates";
            //
            for (int i = 640; i < 649; i++)
                MapBarCode[i] = "Finland";
            //
            for (int i = 690; i < 695; i++)
                MapBarCode[i] = "China";
            //
            for (int i = 700; i < 709; i++)
                MapBarCode[i] = "Norway";
            //
            MapBarCode[729] = "Israel";
            for (int i = 730; i < 739; i++)
                MapBarCode[i] = "Sweden";
            //
            MapBarCode[740] = "Guatemala";
            MapBarCode[741] = "El Salvador";
            MapBarCode[742] = "Honduras";
            MapBarCode[743] = "Nicaragua";
            MapBarCode[744] = "Costa Rica";
            MapBarCode[745] = "Panama";
            MapBarCode[746] = "Dominican Republic";
            MapBarCode[750] = "Mexico";
            MapBarCode[754] = "Canada";
            MapBarCode[755] = "Canada";
            MapBarCode[759] = "Venezuela";
            //
            for (int i = 760; i < 769; i++)
                MapBarCode[i] = "Switzerland";
            //
            MapBarCode[770] = "Colombia";
            MapBarCode[773] = "Uruguay";
            MapBarCode[775] = "Peru";
            MapBarCode[777] = "Bolivia";
            MapBarCode[779] = "Argentina";
            MapBarCode[780] = "Chile";
            MapBarCode[784] = "Paraguay";
            MapBarCode[786] = "Ecuador";
            MapBarCode[789] = "Brazil";
            MapBarCode[790] = "Brazil";
            // 
            for (int i = 800; i < 839; i++)
                MapBarCode[i] = "Italy";
            //
            for (int i = 840; i < 849; i++)
                MapBarCode[i] = "Spain";
            //
            MapBarCode[850] = "Cuba";
            MapBarCode[858] = "Slovakia";
            MapBarCode[859] = "Czech";
            MapBarCode[865] = "Mongolia";
            MapBarCode[867] = "North Korea";
            MapBarCode[868] = "Turkey";
            MapBarCode[869] = "Turkey";
            //
            for (int i = 870; i < 879; i++)
                MapBarCode[i] = "Netherlands";
            //
            MapBarCode[880] = "South Korea";
            MapBarCode[884] = "Cambodia";
            MapBarCode[885] = "Thailand";
            MapBarCode[888] = "Singapore";
            MapBarCode[890] = "India";
            MapBarCode[893] = "VietNam";
            MapBarCode[899] = "Indonesia";
            //
            for (int i = 900; i < 919; i++)
                MapBarCode[i] = "Austria";
            //
            for (int i = 930; i < 939; i++)
                MapBarCode[i] = "Australia";
            //
            for (int i = 940; i < 949; i++)
                MapBarCode[i] = "New Zealand";
            MapBarCode[950] = "Global Office";
            MapBarCode[955] = "Malaysia";
            MapBarCode[958] = "Macau";
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            var window = parameter as AddNewProductWindow;
            var BarcodeContent = window.BarcodeTextBox.Text;
            if (!string.IsNullOrEmpty(BarcodeContent))
            {
                return BarcodeContent.Length >= 3;
            }
            return false;
        }

        public void Execute(object parameter)
        {
            var window = parameter as AddNewProductWindow;
            try
            {
                var number = int.Parse(window.BarcodeTextBox.Text.Substring(0, 3));
                window.CountryTextBlock.Text = MapBarCode[number];
            }
            catch
            {
                window.CountryTextBlock.Text = "Unknow";
            }

            // Bổ sung tính năng:
            // Khi manager nhập đúng barcode của sản phẩm đã tồn tại trong DB
            // Thì tự fill 2 thông tin có sẵn là Title + Image
            var BarcodeContent = window.BarcodeTextBox.Text;

        }
    }
}
