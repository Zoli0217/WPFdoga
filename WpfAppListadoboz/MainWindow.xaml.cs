using Microsoft.Win32;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppListadoboz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public class Muveletek
    {
        public int op1;
        public char muveletiJel;
        public int op2;

        public Muveletek(string[] mezok)
        {
            op1 = Convert.ToInt32(mezok[0]);
            muveletiJel = Convert.ToChar(mezok[1]);
            op2 = Convert.ToInt32(mezok[2]);
        }
    }
    public partial class MainWindow : Window
    {
        Random rnd;
        List<Muveletek> adatok;
        public MainWindow()
        {
            InitializeComponent();
            rnd = new Random();
            adatok = File.ReadAllLines("Műveletek.txt")
            .Select(sor => new Muveletek(sor.Split(" ")))
            .ToList();
        }

        public int Szamol(int a, char op, int b)
        {
            switch (op)
            {
                case '+':
                    return a + b;
                case '-':
                    return a - b;
                case '*':
                    return a * b;
                case '/':
                    return a / b;
                case '%':
                    return a % b;
            }
            return 0;
        }

        private void loadBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();

            string[] beolvas = File.ReadAllLines(ofd.FileName);

            foreach (var item in beolvas)
            {
                lbMuveletek.Items.Add(item);
            }

        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Txt fájl (.txt) | .txt";
            sfd.ShowDialog();

            List<string> eredmenyek = new List<string>();

            foreach (var item in lbEredmenyek.Items)
            {
                eredmenyek.Add(item.ToString());
            }

            File.WriteAllLines(sfd.FileName, eredmenyek);
        }

        private void lbMuveletek_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selected = lbMuveletek.SelectedItem.ToString().Split(" ");
            
            if (lbMuveletek.SelectedItems != null)
            {
                lbEredmenyek.Items.Add($"{selected[0]} {selected[1]} {selected[2]}" + "=" +  Szamol(Convert.ToInt32(selected[0]), Convert.ToChar(selected[1]), Convert.ToInt32(selected[2])));
            }
        }

        private void megoldasBtn_Click(object sender, RoutedEventArgs e)
        {
            int counter = 0;
            foreach (var item in adatok)
            {
                if(item.op1 == 0 ||  item.op2 == 0)
                {
                    counter++;
                }
            }

            lbOutput.Items.Add($"{counter} esetben volt 0 az operandus");
        }

        private void megoldas2Btn_Click(object sender, RoutedEventArgs e)
        {
            adatok.GroupBy(x => x.muveletiJel).ToList().ForEach(g => lbOutput.Items.Add($"{g.Key}: {g.Count()}"));
        }

        private void megoldas3Btn_Click(object sender, RoutedEventArgs e)
        {
            var maxMuvelet = adatok
                .Select(x =>  new { Muvelet = x, Eredmeny = Szamol(x.op1, x.muveletiJel, x.op2) })
                .OrderByDescending(x => x.Eredmeny).First();

            lbOutput.Items.Add(maxMuvelet.Eredmeny);
        }

        private void rndBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();   

            if (sfd.ShowDialog() == true)
            {
                List<string> randomMuveletek = new List<string>();
                char[] muveletiJelek = new char[] { '+', '-', '*', '/', '%' };
                for (int i = 0; i < (int)inputSli.Value; i++)
                {
                    int operandus1 = rnd.Next(1, 100);
                    int operandus2 = rnd.Next(1, 100);

                    char muveletiJel = muveletiJelek[rnd.Next(0, muveletiJelek.Length)];
                    randomMuveletek.Add($"{operandus1} {muveletiJel} {operandus2}");
                }
                File.WriteAllLines(sfd.FileName, randomMuveletek);
            }
        }

        private void inputSli_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            valueTb.Text = inputSli.Value.ToString();
        }
    }

}