import {Component, OnInit} from "@angular/core";
import {ActivatedRoute} from "@angular/router";

@Component({
    selector: "challenge-details",
    template: require("./challenge-details.component.html"),
    styles: [require("./challenge-details.component.css")]
})
export class ChallengeDetailsComponent implements OnInit {
    constructor(private route: ActivatedRoute) { }
    challengeId: string;
    ngOnInit() {
        this.route.params.subscribe(params => {
            this.challengeId = params['id'];
        });
    }
}