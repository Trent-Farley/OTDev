"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const GetNumberFromString_1 = require("../../../MealFridge/Scripts/Fridge/GetNumberFromString");
describe('Fridge Test Suite', function () {
    it("getAmountFromString_ParseString_ReturnInt", function () {
        expect(GetNumberFromString_1.getAmountFromString("1")).toBe(1);
    });
    it("getAmountFromString_ParseStringWithoutNumber_ReturnError", function () {
        expect(GetNumberFromString_1.getAmountFromString("A")).toBe(1);
    });
});
//# sourceMappingURL=TestParseInput.js.map