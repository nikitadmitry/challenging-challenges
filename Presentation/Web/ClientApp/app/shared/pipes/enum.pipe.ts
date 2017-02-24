import { Pipe, PipeTransform } from '@angular/core';
/*
 * Raise the value exponentially
 * Takes an exponent argument that defaults to 1.
 * Usage:
 *   value | exponentialStrength:exponent
 * Example:
 *   {{ 2 |  exponentialStrength:10}}
 *   formats to: 1024
 */
@Pipe({name: 'enum'})
export class EnumPipe implements PipeTransform {
    transform(value: number, enumName: string): string {
        return `${enumName}.${enumName}-${value}`;
    }
}