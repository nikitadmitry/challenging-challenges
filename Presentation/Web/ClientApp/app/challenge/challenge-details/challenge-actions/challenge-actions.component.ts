import {Component, Input, EventEmitter, Output} from "@angular/core";
import {Translation, TranslationService} from "angular-l10n";
import {ChallengeDetailsModel} from "../../models/challenge.model";

@Component({
    selector: "challenge-actions",
    template: require("./challenge-actions.component.html"),
    styles: [require("./challenge-actions.component.css")]
})
export class ChallengeActionsComponent extends Translation {
    @Input() challenge: ChallengeDetailsModel;
    @Output() submit: EventEmitter<void> = new EventEmitter<void>();

    constructor(translationService: TranslationService) {
        super(translationService);

        this.translation.AddConfiguration()
            .AddProvider("./assets/locale-challenges-");
        this.translation.init();
    }
}