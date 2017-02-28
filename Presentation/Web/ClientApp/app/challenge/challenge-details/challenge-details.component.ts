import {Component, OnInit} from "@angular/core";
import {ActivatedRoute} from "@angular/router";
import {ChallengeDetailsModel} from "../models/challenge.model";
import {ChallengesService} from "../../challenges/challenges.service";

@Component({
    selector: "challenge-details",
    template: require("./challenge-details.component.html"),
    styles: [require("./challenge-details.component.css")],
    providers: [ChallengesService]
})
export class ChallengeDetailsComponent implements OnInit {
    challenge: ChallengeDetailsModel;

    constructor(private route: ActivatedRoute, private challengesService: ChallengesService) { }


    ngOnInit() {
        this.route.params.subscribe(params => {
            let challengeId = params['id'];

            this.challengesService.getChallenge(challengeId).subscribe((challenge) => {
                this.challenge = challenge;
            })
        });
    }
}