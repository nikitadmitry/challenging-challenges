import { Component, Input } from "@angular/core";
import { Translation, TranslationService } from "angular-l10n";
import {Router} from "@angular/router";

import { ChallengeCardViewModel } from "./challenge-card.model";

@Component({
    selector: "challenge-card",
    template: require("./challenge-card.component.html")
})
export class ChallengeCardComponent extends Translation {
    @Input()
    challenge: ChallengeCardViewModel;

    constructor(translationService: TranslationService,
        private router: Router){
        super(translationService);
    }

    showAditionalInfo(): void {
        alert("please add details");
    }

    openChallenge(): void {
        this.router.navigate(["./challenge", this.challenge.id]);
    }
}