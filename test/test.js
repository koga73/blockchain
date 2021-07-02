import {expect} from "chai";

import FieldElement from "../src/struct/FieldElement.mjs";
import Point from "../src/struct/Point.mjs";

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

describe("--- Point ---\n", function () {
	const p1 = new Point(-1, -1, 5, 7);
	const p2 = new Point(-1, 1, 5, 7);
	const p3 = new Point(2, 5, 5, 7);
	const inf = new Point(Infinity, Infinity, 5, 7);

	it("equals", function () {
		expect(p1.equals(p1)).to.equal(true);
		expect(p1.equals(p3)).to.equal(false);
	});

	it("add", function () {
		expect(p1.add(p2).x).to.equal(Infinity);
		expect(p1.add(inf).x).to.equal(-1);
		expect(inf.add(p1).x).to.equal(-1);

		const p4 = p1.add(p1);
		expect(p4.x).to.equal(18);
		expect(p4.y).to.equal(77);

		const p5 = p3.add(p1);
		expect(p5.x).to.equal(3);
		expect(p5.y).to.equal(-7);
	});
});
