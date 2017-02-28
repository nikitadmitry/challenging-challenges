import { Injectable } from "@angular/core";
import {Http, URLSearchParams} from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/share";

import { Actions } from "../shared/actions";
import { ChallengesSearchOptions } from "./models/ChallengesSearchOptions";
import {ChallengeDetailsModel} from "../challenge/models/challenge.model";
import {AuthHttp} from "angular2-jwt";

@Injectable()
export class ChallengesService {

    constructor(private http: Http, private authHttp: AuthHttp) { }

    getChallengesCount(): Observable<number> {
        return this.http.get(Actions.home.getChallengesCount)
            .map(response => response.json() as number).share();
    }

    search(searchOptions: ChallengesSearchOptions): Observable<any> {
        return this.http.post(Actions.challenges.searchChallenges, searchOptions)
            .map(response => response.json());
    }

    getChallenge(challengeId: string): Observable<ChallengeDetailsModel> {
        let params = new URLSearchParams();
        params.set('challengeId', challengeId);

        return this.authHttp.get(Actions.challenges.getChallenge, { search: params })
            .map(response => response.json() as ChallengeDetailsModel);
    }
}