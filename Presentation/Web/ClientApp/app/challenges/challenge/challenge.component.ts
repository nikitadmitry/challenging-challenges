import { Component, Input } from "@angular/core";
import { TranslationService, Translation } from "angular-l10n";

import { ChallengeInfoViewModel } from "../models/ChallengeInfoViewModel";

@Component({
    selector: "challenge",
    template: require("./challenge.component.html")
})
export class ChallengeComponent extends Translation {
    @Input()
    challenge: ChallengeInfoViewModel;

    constructor(translationService: TranslationService) {
        super(translationService);
    }

}