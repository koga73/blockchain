class FieldElement {
	constructor(num, prime) {
		if (num >= prime || num < 0) {
			throw new Error(`Number is not in range of 0 to ${prime}`);
		}
		this.num = num;
		this.prime = prime;
	}

	equals(fieldElement) {
		return fieldElement && fieldElement.num === this.num && fieldElement.prime === this.prime;
	}

	add(fieldElement) {
		if (fieldElement.prime !== this.prime) {
			throw new Error("Cannot manipulate fieldElements of different sets");
		}
		return new FieldElement((this.num + fieldElement.num) % this.prime, this.prime);
	}

	subtract(fieldElement) {
		if (fieldElement.prime !== this.prime) {
			throw new Error("Cannot manipulate fieldElements of different sets");
		}
		var num = (this.num - fieldElement.num) % this.prime;
		num = num < 0 ? num + this.prime : num;
		return new FieldElement(num, this.prime);
	}

	multiply(fieldElement) {
		if (fieldElement.prime !== this.prime) {
			throw new Error("Cannot manipulate fieldElements of different sets");
		}
		return new FieldElement((this.num * fieldElement.num) % this.prime, this.prime);
	}

	pow(exponent = 2) {
		const n = exponent % (this.prime - 1);
		return new FieldElement(Math.pow(this.num, n) % this.prime, this.prime);
	}

	divide(fieldElement) {
		if (fieldElement.prime !== this.prime) {
			throw new Error("Cannot manipulate fieldElements of different sets");
		}
		return this.multiply(fieldElement.pow(fieldElement.prime - 2));
	}

	toString() {
		return `{num:${num}, prime:${prime}}`;
	}
}
export default FieldElement;
