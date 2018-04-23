using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Resources;
using System.Xml.Linq;
using DevExpress.Utils.Filtering;
using DevExpress.Xpf.Core;

namespace ChartFiltering {
    public partial class MainWindow : ThemedWindow {
        #region #MainWindow_Ctor
        public MainWindow() {
            InitializeComponent();

            StreamResourceInfo dataStreamInfo = Application.GetResourceStream(new Uri("Data/GDPStatistic.xml", UriKind.RelativeOrAbsolute));
            XmlGdpValueProvider valueProvider = new XmlGdpValueProvider(dataStreamInfo.Stream);
            DataContext = new MainViewModel(valueProvider);
        }
        #endregion #MainWindow_Ctor
    }

    #region #MainViewModel
    public class MainViewModel {
        public IEnumerable<GdpValue> GdpValues { get; private set; }

        //Filter parameters
        public double MinGdpValue { get; private set; }
        public double MaxGdpValue { get; private set; }
        public int StartYear { get; private set; }
        public int EndYear { get; private set; }
        public IEnumerable<string> CountryNames { get; private set; }
        public IEnumerable<string> ContinentNames { get; private set; }


        public MainViewModel(XmlGdpValueProvider valueProvider) {
            GdpValues = valueProvider.GetValues();
            MinGdpValue = GdpValues.Min(gdp => gdp.Value);
            MaxGdpValue = GdpValues.Max(gdp => gdp.Value);
            StartYear = GdpValues.Min(gdp => gdp.Year);
            EndYear = GdpValues.Max(gdp => gdp.Year);
            CountryNames = GdpValues.Select(gdp => gdp.CountryName);
            ContinentNames = GdpValues.Select(gdp => gdp.ContinentName);

        }
    }
    #endregion #MainViewModel

    public class XmlGdpValueProvider {
        Stream xmlStream;

        public XmlGdpValueProvider(Stream xmlStream) {
            this.xmlStream = xmlStream;
        }

        public IEnumerable<GdpValue> GetValues() {
            XElement dataSet = XDocument.Load(xmlStream).Element("data-set");
            List<GdpValue> values = new List<GdpValue>();
            foreach(XElement continent in dataSet.Elements("ContinentInfo")) {
                String continentName = continent.Element("ContinentName").Value;
                foreach(XElement country in continent.Element("Countries").Elements("CountryInfo")) {
                    String countryName = country.Element("Name").Value;
                    foreach(XElement gdpByYear in country.Element("Statistic").Elements("GDPByYear")) {
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

    #region #GdpValue
    public class GdpValue {
        [FilterLookup(DataSourceMember = "ContinentNames", UseBlanks = false)]
        public string ContinentName { get; set; }

        [FilterLookup(DataSourceMember = "CountryNames", UseBlanks = false)]
        public string CountryName { get; set; }

        [FilterRange(MaximumMember = "MaxGdpValue", MinimumMember = "MinGdpValue")]
        public double Value { get; set; }

        [FilterRange(MinimumMember = "StartYear", MaximumMember = "EndYear")]
        public int Year { get; set; }
    }
    #endregion #GdpValue
}
