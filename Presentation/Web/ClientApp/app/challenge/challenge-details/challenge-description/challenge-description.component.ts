import {Component, Input} from "@angular/core";
import {Translation, TranslationService} from "angular-l10n";
import {ChallengeDetailsModel} from "../../models/challenge.model";

@Component({
    selector: "challenge-description",
    template: require("./challenge-description.component.html"),
    styles: [require("./challenge-description.component.css")]
})
export class ChallengeDescriptionComponent extends Translation {
    @Input() challenge: ChallengeDetailsModel;

    constructor(translationService: TranslationService) {
        super(translationService);

        this.translation.AddConfiguration()
            .AddProvider("./assets/locale-challenges-");
        this.translation.init();
    }

    openAuthor() {
        alert('opened user with id ' + this.challenge.authorId);
    }
}