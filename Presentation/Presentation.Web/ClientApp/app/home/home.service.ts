import { Injectable } from "@angular/core";
import { Http } from "@angular/http";
import { Observable } from "rxjs/Observable";

import { ChallengeCardViewModel } from "./challenges/challenge-card.model";
import { SortedPageRule } from "./challenges/models/SortedPageRule";
import { Actions } from "../shared/actions";
import "rxjs/add/operator/share";
import "rxjs/add/operator/delay";

@Injectable()
export class HomeService {

    constructor(private http: Http) { }

    getChallenges(sortedPageRule: SortedPageRule): Observable<ChallengeCardViewModel[]> {
        return this.http.post(Actions.home.getChallenges, sortedPageRule)
            .map(response => response.json() as ChallengeCardViewModel[]).share();
    }

    getTopUsers(): Observable<any[]> {
        return this.http.get(Actions.home.getTopUsers)
            .map(response => response.json() as any[]);
    }
}