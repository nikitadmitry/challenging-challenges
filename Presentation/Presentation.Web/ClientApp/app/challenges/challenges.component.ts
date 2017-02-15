import { Component, OnInit, ViewChild } from "@angular/core";
import { MdlSelectComponent } from "@angular2-mdl-ext/select";
import { PaginationInstance } from "ng2-pagination";
import { Translation, TranslationService } from "angular-l10n";

import { ChallengesService } from "../challenges/challenges.service";
import { ChallengeSearchType } from "./models/ChallengeSearchType";
import { ChallengesPageRule } from "./models/ChallengesPageRule";

@Component({
    selector: "challenges",
    template: require("./challenges.component.html"),
    styles: [require("./challenges.component.css")],
    providers: [ChallengesService]
})
export class ChallengesComponent extends Translation implements OnInit {
    private PAGE_SIZE: number = 10;
    selectedSearchType: ChallengeSearchType = ChallengeSearchType.Title;
    searchString: string;
    searchTypes: any[];
    challenges: any[];
    isLoading: boolean = true;
    @ViewChild(MdlSelectComponent) searchTypeSelect: MdlSelectComponent;
    config: PaginationInstance = {
        id: "search-challenges-paginator",
        itemsPerPage: this.PAGE_SIZE,
        currentPage: 1
    };

    constructor(private challengesService: ChallengesService, translationService: TranslationService) {
        super(translationService);

        this.translation.AddConfiguration()
            .AddProvider("./assets/locale-challenges-");
        this.translation.init();

        this.translation.translationChanged.subscribe(() => {
            this.initializeSearchTypes();
            this.selectedSearchType = ChallengeSearchType.Title;
        });
    }

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

    private initializeSearchTypes(): void {
        this.searchTypes = [
            {type: ChallengeSearchType.Title, name: this.translation.translate("Title")},
            {type: ChallengeSearchType.Condition, name: "Условие"},
            {type: ChallengeSearchType.Difficulty, name: "Сложность"},
            {type: ChallengeSearchType.Language, name: "Язык"},
            {type: ChallengeSearchType.PreviewText, name: "Текст Превью"},
            {type: ChallengeSearchType.Section, name: "Раздел"},
            {type: ChallengeSearchType.Tags, name: "Тэги"}
        ];
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
