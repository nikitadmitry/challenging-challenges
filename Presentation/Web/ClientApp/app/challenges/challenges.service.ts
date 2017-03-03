import { Injectable } from "@angular/core";
import {Http, URLSearchParams, RequestOptions, Headers} from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/share";

import { Actions } from "../shared/actions";
import { ChallengesSearchOptions } from "./models/ChallengesSearchOptions";
import {ChallengeDetailsModel} from "../challenge/models/challenge.model";
import {AuthHttp} from "angular2-jwt";
import {SourceCodeTemplate} from "./models/SourceCodeTemplate";
import {Section} from "../challenge/models/Section";

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

    solve(challengeId: string, answer: string): Observable<any> {
        var headers = new Headers();
        headers.append('Content-Type', 'application/json');

        let params = new URLSearchParams();
        params.set('challengeId', challengeId);

        return this.authHttp.post(Actions.challenges.solve, JSON.stringify(answer), { search: params, headers: headers })
            .map(response => response.json());
    }

    getSourceCodeTemplate(section: Section): Observable<string> {
        let params = new URLSearchParams();
        params.set('section', section.toString());

        return this.http.get(Actions.challenges.getSourceCodeTemplate, { search: params })
            .map(response => response.text() as string);
    }

    getChallenge(challengeId: string): Observable<ChallengeDetailsModel> {
        let params = new URLSearchParams();
        params.set('challengeId', challengeId);

        return this.authHttp.get(Actions.challenges.getChallenge, { search: params })
            .map(response => response.json() as ChallengeDetailsModel);
    }
}