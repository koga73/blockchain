import {expect} from "chai";

import FieldElement from "../src/struct/FieldElement.mjs";

describe("--- FieldElement ---\n", function () {
	const fe1 = new FieldElement(3, 7);
	const fe2 = new FieldElement(3, 7);
	const fe3 = new FieldElement(5, 7);

	it("equals", function () {
		expect(fe1.equals(fe2)).to.equal(true);
		expect(fe1.equals(fe3)).to.equal(false);
	});

	it("addition", function () {
		expect(fe1.add(fe2).num).to.equal(6);
		expect(fe1.add(fe3).num).to.equal(1);
	});

	it("subtraction", function () {
		expect(fe1.subtract(fe2).num).to.equal(0);
		expect(fe1.subtract(fe3).num).to.equal(5);
	});

	it("multiplication", function () {
		expect(fe1.multiply(fe2).num).to.equal(2);
		expect(fe1.multiply(fe3).num).to.equal(1);
	});

	it("division", function () {
		const fe4 = new FieldElement(2, 19);
		const fe5 = new FieldElement(5, 19);
		const fe6 = new FieldElement(7, 19);

		expect(fe4.divide(fe6).num).to.equal(3);
		expect(fe6.divide(fe5).num).to.equal(9);
	});
});
