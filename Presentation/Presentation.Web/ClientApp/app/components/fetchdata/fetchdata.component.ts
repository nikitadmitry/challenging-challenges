import { Component, OnInit } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'fetchdata',
    template: require('./fetchdata.component.html')
})
export class FetchDataComponent implements OnInit {
    public forecasts: IWeatherForecast[];

    constructor(private http: Http) {
        
    }

    public ngOnInit() {
        this.http.get('/api/SampleData/WeatherForecasts').subscribe(result => {
            this.forecasts = result.json() as IWeatherForecast[];
        });
    }
}

interface IWeatherForecast {
    dateFormatted: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}
