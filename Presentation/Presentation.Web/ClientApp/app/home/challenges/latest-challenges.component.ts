import { Component } from "@angular/core";

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
    protected componentTitle: string = "Последние задачи";
    protected paginatorId: string = "latest-challenges-paginator";

    constructor(homeService: HomeService) {
        super(homeService);
    }
}