import { Component, OnInit } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { TranslationService, Translation } from "angular-l10n";

import { ChallengesService } from "../challenges/challenges.service";

@Component({
    selector: "home",
    template: require("./home.component.html"),
    providers: [ChallengesService]
})
export class HomeComponent extends Translation implements OnInit {
    challengesCount: Observable<number>;

    constructor(translation: TranslationService, private challengesService: ChallengesService) {
        super(translation);
    }

    ngOnInit(): void {
        this.challengesCount = this.challengesService.getChallengesCount();
    }
}
