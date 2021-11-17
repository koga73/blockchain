import * as crypto from "crypto";

class Block {
	constructor({prevHash, transaction, timestamp = Date.now()}) {
		this.prevHash = prevHash;
		this.transaction = transaction;
		this.timestamp = timestamp;

		this.nonce = (Math.random() * 999999999) >> 0;
	}

	getHash() {
		const str = JSON.stringify(this);
		const hash = crypto.createHash("SHA256");
		hash.update(str).end();
		return hash.digest("hex");
	}
}
export default Block;
