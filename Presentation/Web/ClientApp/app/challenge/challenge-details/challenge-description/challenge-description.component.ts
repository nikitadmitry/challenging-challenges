import {Component, Input} from "@angular/core";
import {Translation, TranslationService} from "angular-l10n";
import {ChallengeDetailsModel} from "../../models/challenge.model";
import {Router} from "@angular/router";

@Component({
    selector: "challenge-description",
    template: require("./challenge-description.component.html"),
    styles: [require("./challenge-description.component.css")]
})
export class ChallengeDescriptionComponent extends Translation {
    @Input() challenge: ChallengeDetailsModel;

    constructor(translationService: TranslationService, private router: Router) {
        super(translationService);

        this.translation.AddConfiguration()
            .AddProvider("./assets/locale-challenges-");
        this.translation.init();
    }

    openAuthor() {
        this.router.navigate(["user", this.challenge.authorId]);
    }
}