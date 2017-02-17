import { Component } from "@angular/core";
import { TranslationService } from "angular-l10n";

import { ChallengesComponent } from "./challenges.component";
import { SortingType } from "./models/SortingType";
import { HomeService } from "../home.service";

@Component({
    selector: "latest-challenges",
    template: require("./challenges.component.html"),
    styles: [require("./challenges.component.css")],
    providers: [HomeService]
})
export class LatestChallengesComponent extends ChallengesComponent {
    protected sortingType: SortingType = SortingType.Latest;
    protected componentTitleId: string = "Home.LatestChallenges";
    protected paginatorId: string = "latest-challenges-paginator";

    constructor(homeService: HomeService, translationService: TranslationService) {
        super(homeService, translationService);
    }
}