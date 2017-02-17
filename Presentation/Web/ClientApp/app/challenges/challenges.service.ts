import { Injectable } from "@angular/core";
import { Http } from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/share";

import { Actions } from "../shared/actions";
import { ChallengesPageRule } from "./models/ChallengesPageRule";

@Injectable()
export class ChallengesService {

    constructor(private http: Http) { }

    getChallengesCount(): Observable<number> {
        return this.http.get(Actions.home.getChallengesCount)
            .map(response => response.json() as number).share();
    }

    search(pageRule: ChallengesPageRule): Observable<any> {
        return this.http.post(Actions.challenges.searchChallenges, pageRule)
            .map(response => response.json());
    }
}