import { Component, Inject, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import {
  ChartComponent,
  ApexAxisChartSeries,
  ApexChart,
  ApexXAxis,
  ApexDataLabels,
  ApexTitleSubtitle,
  ApexStroke,
  ApexGrid
} from "ng-apexcharts";

export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  dataLabels: ApexDataLabels;
  grid: ApexGrid;
  stroke: ApexStroke;
  title: ApexTitleSubtitle;
};


@Component({
  selector: 'app-subscriber',
  templateUrl: './subscriber.component.html'
})
export class FetchDataComponent {
  @ViewChild("chart") chart: ChartComponent;
  public chartOptions: Partial<ChartOptions> |any;

  public exampleMsg: ExampleMsg[] = [];
  private baseUrl: string;
  private httpClient: HttpClient;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.httpClient = http;
    this.baseUrl = baseUrl;


    setInterval(() => {
      this.httpClient.get<ExampleMsg[]>(this.baseUrl + 'subscriber').subscribe(result => {
        this.exampleMsg = result;
        var mapped = result.map(v => v.m_temperature);
        var seqmapped = result.map(v => v.m_sequenceNumber);

        this.chart.updateSeries([
          {
            data: mapped
          }]);
        
        this.chart.updateOptions({
          xaxis: {
            categories: seqmapped,
          }
        })


      }, error => console.error(error));
    }, 500)

    this.chartOptions = {
      series: [
        {
          name: "Temperature",
          data: []
        }
      ],
      chart: {
        height: 350,
        type: "line",
        zoom: {
          enabled: false
        }
      },
      dataLabels: {
        enabled: true
      },
      stroke: {
        curve: "straight"
      },
      title: {
        text: "Example Temperature readings",
        align: "left"
      },
      grid: {
        row: {
          colors: ["#f3f3f3", "transparent"], // takes an array which will be repeated on columns
          opacity: 0.5
        }
      }
    };


  }
}

interface ExampleMsg {
  m_sequenceNumber: number;
  m_temperature: number;
}
