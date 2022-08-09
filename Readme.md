<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128569908/21.1.5%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T568127)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->

# Charts for WPF - Create Filter UI to Filter Series Data

The following example demonstrates how to use [FilterBehavior](https://docs.devexpress.com/WPF/DevExpress.Xpf.Core.FilteringUI.FilterBehavior?p=netframework) and [AccordionControl](https://docs.devexpress.com/WPF/118347/controls-and-libraries/navigation-controls/accordion-control?p=netframework) to create a custom Filtering UI for chart data. To apply a filter, bind a series' [Series.FilterCriteria](https://docs.devexpress.com/WPF/DevExpress.Xpf.Charts.Series.FilterCriteria?p=netframework) property to the [FilterBehavior.ActualFilterCriteria](https://docs.devexpress.com/WPF/DevExpress.Xpf.Core.FilteringUI.FilterBehavior.ActualFilterCriteria?p=netframework) property value. In this example, an accordion control contains [filter elements](https://docs.devexpress.com/WPF/DevExpress.Xpf.Core.FilteringUI.FilterElement).

![Resulting chart](Images/results.png)

## Files to Look At

* [MainWindow.xaml](./CS/ChartFiltering/MainWindow.xaml) (VB: [MainWindow.xaml](./VB/ChartFiltering/MainWindow.xaml))
* [MainWindow.xaml.cs](./CS/ChartFiltering/MainWindow.xaml.cs) (VB: [MainWindow.xaml.vb](./VB/ChartFiltering/MainWindow.xaml.vb))

## Documentation

* [Chart Control - Filter Chart Data](https://docs.devexpress.com/WPF/116571/controls-and-libraries/charts-suite/chart-control/filter-data?p=netframework)
* [Filter Behavior - Filter Elements and ChartControl](https://docs.devexpress.com/WPF/DevExpress.Xpf.Core.FilteringUI.FilterBehavior?p=netframework#filter-elements-and-chartcontrol)

## More Examples

* [How to filter series data](https://github.com/DevExpress-Examples/how-to-filter-series-data-t339646)
* [How to sort qualitative values in a custom order](https://github.com/DevExpress-Examples/how-to-sort-qualitative-values-in-a-custom-order-t318834)
