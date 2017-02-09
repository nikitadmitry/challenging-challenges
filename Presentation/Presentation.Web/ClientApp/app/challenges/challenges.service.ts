import { Injectable } from "@angular/core";
import { Http } from "@angular/http";
import { Observable } from "rxjs/Observable";

import { Actions } from "../shared/actions";
import "rxjs/add/operator/share";
import "rxjs/add/operator/delay";

@Injectable()
export class ChallengesService {

    constructor(private http: Http) { }

    getChallengesCount(): Observable<number> {
        return this.http.get(Actions.home.getChallengesCount)
            .map(response => response.json() as number).share();
    }
}