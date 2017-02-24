import { Component, Input, Output, EventEmitter } from "@angular/core";
import { Translation, TranslationService } from "angular-l10n";

import { ChallengeSearchType } from "../models/ChallengeSearchType";

@Component({
    selector: "filters",
    template: require("./filters.component.html"),
    styles: [require("./filters.component.css")]
})
export class FiltersComponent extends Translation {
    @Input() searchType: ChallengeSearchType;
    @Output() searchTypeChange: EventEmitter<ChallengeSearchType> = new EventEmitter<ChallengeSearchType>();

    @Input() searchString: string;
    @Output() searchStringChange: EventEmitter<string> = new EventEmitter<string>();

    searchTypes: any[];
    @Output() changeFilter: EventEmitter<void> = new EventEmitter<void>();

    constructor(translationService: TranslationService) {
        super(translationService);

        this.translation.translationChanged.subscribe(() => {
            this.initializeSearchTypes();
        });
    }

    submit(): void {

        this.changeFilter.emit();
    }

    private initializeSearchTypes(): void {
        this.searchTypes = [
            {type: ChallengeSearchType.Title, name: this.translation.translate("Title")},
            {type: ChallengeSearchType.Condition, name: this.translation.translate("Condition")},
            {type: ChallengeSearchType.Difficulty, name: this.translation.translate("Difficulty")},
            {type: ChallengeSearchType.Language, name: this.translation.translate("Language")},
            {type: ChallengeSearchType.PreviewText, name: this.translation.translate("PreviewText")},
            {type: ChallengeSearchType.Section, name: this.translation.translate("Section")},
            {type: ChallengeSearchType.Tags, name: this.translation.translate("Tags")}
        ];
    }
}