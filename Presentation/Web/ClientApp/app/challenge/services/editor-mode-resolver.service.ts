import {Injectable} from "@angular/core";
import {Section} from "../models/Section";

@Injectable()
export class EditorModeResolver {
    public resolve(section: Section): string {
        switch (section) {
            case Section.CSharp: return "csharp";
            case Section.Java: return "java";
            case Section.Python: return "python";
            case Section.Ruby: return "ruby";
            case Section.Other: return "text";
        }
    }
}