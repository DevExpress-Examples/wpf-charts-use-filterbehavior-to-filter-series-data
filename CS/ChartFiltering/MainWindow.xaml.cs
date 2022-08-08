using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Resources;
using System.Xml.Linq;

namespace ChartFiltering {
    public partial class MainWindow : ThemedWindow {
        public MainWindow() {
            InitializeComponent();

            StreamResourceInfo dataStreamInfo = Application.GetResourceStream(new Uri("Data/GDPStatistic.xml", UriKind.RelativeOrAbsolute));
            XmlGdpValueProvider valueProvider = new XmlGdpValueProvider(dataStreamInfo.Stream);
            DataContext = new MainViewModel(valueProvider);
        }
    }

    public class MainViewModel {
        public IEnumerable<GdpValue> GdpValues { get; private set; }

        public MainViewModel(XmlGdpValueProvider valueProvider) {
            GdpValues = valueProvider.GetValues();
        }
    }

    public class XmlGdpValueProvider {
        Stream xmlStream;

        public XmlGdpValueProvider(Stream xmlStream) {
            this.xmlStream = xmlStream;
        }

        public IEnumerable<GdpValue> GetValues() {
            XElement dataSet = XDocument.Load(xmlStream).Element("data-set");
            List<GdpValue> values = new List<GdpValue>();
            foreach (XElement continent in dataSet.Elements("ContinentInfo")) {
                String continentName = continent.Element("ContinentName").Value;
                foreach (XElement country in continent.Element("Countries").Elements("CountryInfo")) {
                    String countryName = country.Element("Name").Value;
                    foreach (XElement gdpByYear in country.Element("Statistic").Elements("GDPByYear")) {
                        int year = int.Parse(gdpByYear.Element("Year").Value);
                        double value = String.IsNullOrEmpty(gdpByYear.Element("GDP").Value)
                            ? 0
                            : double.Parse(gdpByYear.Element("GDP").Value);
                        values.Add(new GdpValue {
                            ContinentName = continentName,
                            CountryName = countryName,
                            Year = year,
                            Value = value / 1000000000000.0
                        });
                    }
                }
            }

            return values;
        }
    }
    public class GdpValue {
        public string ContinentName { get; set; }
        public string CountryName { get; set; }
        public double Value { get; set; }
        public int Year { get; set; }
    }
}
