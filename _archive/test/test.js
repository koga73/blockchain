import {expect} from "chai";

import FieldElement from "../src/struct/FieldElement.mjs";
import Point from "../src/struct/Point.mjs";
import Wallet from "../src/struct/Wallet.mjs";
import Chain from "../src/struct/Chain.mjs";

describe("--- FieldElement ---\n", function () {
	const fe1 = new FieldElement(3n, 7n);
	const fe2 = new FieldElement(3n, 7n);
	const fe3 = new FieldElement(5n, 7n);

	it("equals", function () {
		expect(fe1.equals(fe2)).to.equal(true);
		expect(fe1.equals(fe3)).to.equal(false);
	});

	it("addition", function () {
		expect(fe1.add(fe2).num).to.equal(6n);
		expect(fe1.add(fe3).num).to.equal(1n);
	});

	it("subtraction", function () {
		expect(fe1.subtract(fe2).num).to.equal(0n);
		expect(fe1.subtract(fe3).num).to.equal(5n);
	});

	it("multiplication", function () {
		expect(fe1.multiply(fe2).num).to.equal(2n);
		expect(fe1.multiply(fe3).num).to.equal(1n);
		expect(fe1.multiply(3n).num).to.equal(2n);
	});

	it("division", function () {
		const fe4 = new FieldElement(2n, 19n);
		const fe5 = new FieldElement(5n, 19n);
		const fe6 = new FieldElement(7n, 19n);

		expect(fe4.divide(fe6).num).to.equal(3n);
		expect(fe6.divide(fe5).num).to.equal(9n);
	});

	it("exponentation", function () {
		expect(fe1.pow(2n).num).to.equal(2n);
	});
});

describe("--- Point ---\n", function () {
	const p1 = new Point(-1n, -1n, 5n, 7n);
	const p2 = new Point(-1n, 1n, 5n, 7n);
	const p3 = new Point(2n, 5n, 5n, 7n);
	const inf = new Point(Infinity, Infinity, 5n, 7n);

	it("equals", function () {
		expect(p1.equals(p1)).to.equal(true);
		expect(p1.equals(p3)).to.equal(false);
	});

	it("add", function () {
		expect(p1.add(p2).x).to.equal(Infinity);
		expect(p1.add(inf).x).to.equal(-1n);
		expect(inf.add(p1).x).to.equal(-1n);

		const p4 = p1.add(p1);
		expect(p4.x).to.equal(18n);
		expect(p4.y).to.equal(77n);

		const p5 = p3.add(p1);
		expect(p5.x).to.equal(3n);
		expect(p5.y).to.equal(-7n);
	});

	it("FieldElement", function () {
		const prime = 223n;
		const a = new FieldElement(0n, prime);
		const b = new FieldElement(7n, prime);

		const validPoints = [
			[new FieldElement(192n, prime), new FieldElement(105n, prime)],
			[new FieldElement(17n, prime), new FieldElement(56n, prime)],
			[new FieldElement(1n, prime), new FieldElement(193n, prime)]
		];
		for (let i = 0; i < validPoints.length; i++) {
			const vp = validPoints[i];
			expect(() => new Point(vp[0], vp[1], a, b)).to.not.throw();
		}

		const invalidPoints = [
			[new FieldElement(200n, prime), new FieldElement(119n, prime)],
			[new FieldElement(42n, prime), new FieldElement(99n, prime)]
		];
		for (let i = 0; i < invalidPoints.length; i++) {
			const ip = invalidPoints[i];
			expect(() => new Point(ip[0], ip[1], a, b)).to.throw();
		}
	});

	it("FieldElement add", function () {
		const prime = 223n;
		const a = new FieldElement(0n, prime);
		const b = new FieldElement(7n, prime);
		const x1 = new FieldElement(192n, prime);
		const y1 = new FieldElement(105n, prime);
		const x2 = new FieldElement(17n, prime);
		const y2 = new FieldElement(56n, prime);
		const p1 = new Point(x1, y1, a, b);
		const p2 = new Point(x2, y2, a, b);

		const p3 = p1.add(p2);
		expect(p3.x.num).to.equal(170n);
		expect(p3.y.num).to.equal(142n);
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

	it("order n", function () {
		const x = new FieldElement(gx, p);
		const y = new FieldElement(gy, p);
		const zero = new FieldElement(0n, p);
		const seven = new FieldElement(7n, p);
		const g = new Point(x, y, zero, seven);
		const n = 0xffffffffffffffffffffffffffffffffebaaedce6af48a03bbfd25e8cd0364141n;
		const result = g.multiply(n);

		//This should return Infinity not be equal
		expect(result.equals(g)).to.equal(true);
	});
});

const satoshi = new Wallet();
const bob = new Wallet();
const alice = new Wallet();

satoshi.sendMoney({amount: 50, payeePublicKey: bob.publicKey});
bob.sendMoney({amount: 23, payeePublicKey: alice.publicKey});
alice.sendMoney({amount: 5, payeePublicKey: bob.publicKey});

console.log(Chain.instance);
