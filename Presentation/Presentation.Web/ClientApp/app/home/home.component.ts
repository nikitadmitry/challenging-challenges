import { Component, OnInit } from "@angular/core";
import { Observable } from "rxjs/Observable";

import { ChallengesService } from "../challenges/challenges.service";

@Component({
    selector: "home",
    template: require("./home.component.html"),
    providers: [ChallengesService]
})
export class HomeComponent implements OnInit {
    constructor(private challengesService: ChallengesService) { }

    challengesCount: Observable<number>;

    ngOnInit(): void {
        this.challengesCount = this.challengesService.getChallengesCount();
    }
}
