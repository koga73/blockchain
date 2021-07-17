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
		expect(fe1.multiply(3).num).to.equal(2);
	});

	it("division", function () {
		const fe4 = new FieldElement(2, 19);
		const fe5 = new FieldElement(5, 19);
		const fe6 = new FieldElement(7, 19);

		expect(fe4.divide(fe6).num).to.equal(3);
		expect(fe6.divide(fe5).num).to.equal(9);
	});

	it("exponentation", function () {
		expect(fe1.pow(2).num).to.equal(2);
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

	it("FieldElement", function () {
		const prime = 223;
		const a = new FieldElement(0, prime);
		const b = new FieldElement(7, prime);

		const validPoints = [
			[new FieldElement(192, prime), new FieldElement(105, prime)],
			[new FieldElement(17, prime), new FieldElement(56, prime)],
			[new FieldElement(1, prime), new FieldElement(193, prime)]
		];
		for (let i = 0; i < validPoints.length; i++) {
			const vp = validPoints[i];
			expect(() => new Point(vp[0], vp[1], a, b)).to.not.throw();
		}

		const invalidPoints = [
			[new FieldElement(200, prime), new FieldElement(119, prime)],
			[new FieldElement(42, prime), new FieldElement(99, prime)]
		];
		for (let i = 0; i < invalidPoints.length; i++) {
			const ip = invalidPoints[i];
			expect(() => new Point(ip[0], ip[1], a, b)).to.throw();
		}
	});

	it("FieldElement add", function () {
		const prime = 223;
		const a = new FieldElement(0, prime);
		const b = new FieldElement(7, prime);
		const x1 = new FieldElement(192, prime);
		const y1 = new FieldElement(105, prime);
		const x2 = new FieldElement(17, prime);
		const y2 = new FieldElement(56, prime);
		const p1 = new Point(x1, y1, a, b);
		const p2 = new Point(x2, y2, a, b);

		const p3 = p1.add(p2);
		expect(p3.x.num).to.equal(170);
		expect(p3.y.num).to.equal(142);
	});
});

describe("--- secp256k1 ---\n", function () {
	const gx = 0x79be667ef9dcbbac55a06295ce870b07029bfcdb2dce28d959f2815b16f81798n;
	const gy = 0x483ada7726a3c4655da4fbfc0e1108a8fd17b448a68554199c47d08ffb10d4b8n;
	const p = 2n ** 256n - 2n ** 32n - 977n;

	it("on curve", function () {
		const result1 = gy ** 2n % p;
		const result2 = (gx ** 3n + 7n) % p;

		expect(result1 === result2).to.equal(true);
	});
});
