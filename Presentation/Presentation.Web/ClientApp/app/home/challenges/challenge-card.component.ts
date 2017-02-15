import { Component, Input } from "@angular/core";
import { Translation, TranslationService } from "angular-l10n";

import { ChallengeCardViewModel } from "./challenge-card.model";

@Component({
    selector: "challenge-card",
    template: require("./challenge-card.component.html")
})
export class ChallengeCardComponent extends Translation {
    @Input()
    challenge: ChallengeCardViewModel;

    constructor(translationService: TranslationService){
        super(translationService);
    }

    showAditionalInfo(): void {
        alert("please add details");
    }
}