package app.ewarehouse.service;
import app.ewarehouse.entity.BuyerPaymentInvoice;
import app.ewarehouse.entity.Document;


import java.util.List;
import java.util.Optional;

public interface BuyerPaymentInvoiceService {

	String saveData(String data);

	Optional<BuyerPaymentInvoice> getInvoiceByNumber(String invoiceNumber);

	Document saveDocument(Document document);

}

