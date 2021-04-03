import { getAmountFromString } from "../../../MealFridge/Scripts/Fridge/GetNumberFromString"

describe('Fridge Test Suite', function () {
    it("getAmountFromString_ParseString_ReturnInt", function () {
        expect(getAmountFromString("1")).toBe(1);
    });
    it("getAmountFromString_ParseStringWithoutNumber_ReturnNaN", function () {
        expect(getAmountFromString("A")).toBe(NaN);
    });
});
