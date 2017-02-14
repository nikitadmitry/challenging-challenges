import { Component, OnInit, ViewChild } from "@angular/core";
import { MdlSelectComponent } from "@angular2-mdl-ext/select";
import { PaginationInstance } from "ng2-pagination";

import { ChallengesService } from "../challenges/challenges.service";
import { ChallengeSearchType } from "./models/ChallengeSearchType";
import { ChallengesPageRule } from "./models/ChallengesPageRule";

@Component({
    selector: "challenges",
    template: require("./challenges.component.html"),
    styles: [require("./challenges.component.css")],
    providers: [ChallengesService]
})
export class ChallengesComponent implements OnInit {
    private PAGE_SIZE: number = 10;
    selectedSearchType: ChallengeSearchType = ChallengeSearchType.Title;
    searchString: string;
    searchTypes = [
        {type: ChallengeSearchType.Title, name: "Название"},
        {type: ChallengeSearchType.Condition, name: "Условие"},
        {type: ChallengeSearchType.Difficulty, name: "Сложность"},
        {type: ChallengeSearchType.Language, name: "Язык"},
        {type: ChallengeSearchType.PreviewText, name: "Текст Превью"},
        {type: ChallengeSearchType.Section, name: "Раздел"},
        {type: ChallengeSearchType.Tags, name: "Тэги"}
    ];
    @ViewChild(MdlSelectComponent) searchTypeSelect: MdlSelectComponent;
    config: PaginationInstance = {
        id: "search-challenges-paginator",
        itemsPerPage: this.PAGE_SIZE,
        currentPage: 1
    };
    challenges: any[];
    isLoading: boolean = true;

    constructor(private challengesService: ChallengesService) { }

    ngOnInit(): void {
        this.challengesService.getChallengesCount()
            .subscribe(count => {
                this.config.totalItems = count;
                this.searchChallenges();
            });
    }

    searchChallenges(): void {
        this.isLoading = true;
        this.challengesService.search(this.getPageRule()).subscribe(challenges => {
            this.challenges = challenges;
            this.isLoading = false;
        });
    }

    private getPageRule(): ChallengesPageRule {
        let pageRule: ChallengesPageRule = new ChallengesPageRule();
        pageRule.count = this.PAGE_SIZE;
        pageRule.start = (this.config.currentPage - 1) * this.PAGE_SIZE;
        pageRule.keyword = this.searchString;
        pageRule.searchTypes = [this.selectedSearchType];

        return pageRule;
    }
}
