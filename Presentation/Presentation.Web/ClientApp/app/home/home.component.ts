import { Component, OnInit } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { TranslationService } from "angular-l10n";

import { ChallengesService } from "../challenges/challenges.service";

@Component({
    selector: "home",
    template: require("./home.component.html"),
    providers: [ChallengesService]
})
export class HomeComponent implements OnInit {
    challengesCount: Observable<number>;

    constructor(translation: TranslationService, private challengesService: ChallengesService) {
        translation.AddConfiguration().AddProvider("./assets/locale-home-");
        translation.init();
    }

    ngOnInit(): void {
        this.challengesCount = this.challengesService.getChallengesCount();
    }
}
