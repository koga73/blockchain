import * as crypto from "crypto";

import Chain from "./Chain.mjs";
import Transaction from "./Transaction.mjs";

class Wallet {
	constructor() {
		const keypair = crypto.generateKeyPairSync("rsa", {
			modulusLength: 2048,
			publicKeyEncoding: {type: "spki", format: "pem"},
			privateKeyEncoding: {type: "pkcs8", format: "pem"}
		});
		this.publicKey = keypair.publicKey;
		this.privateKey = keypair.privateKey;
	}

	sendMoney({amount, payeePublicKey}) {
		const transaction = new Transaction({amount, payer: this.publicKey, payee: payeePublicKey});
		const sign = crypto.createSign("SHA256");
		sign.update(transaction.toString()).end();
		const signature = sign.sign(this.privateKey);
		Chain.instance.addBlock({transaction, senderPublicKey: this.publicKey, signature});
	}
}
export default Wallet;
