package app.ewarehouse.serviceImpl;

import app.ewarehouse.dto.BuyerPaymentInvoiceRequest;
import app.ewarehouse.entity.BuyerDepositor;
import app.ewarehouse.entity.BuyerPaymentInvoice;
import app.ewarehouse.entity.Document;
import app.ewarehouse.entity.TwarehouseReceipt;
import app.ewarehouse.exception.CustomGeneralException;
import app.ewarehouse.repository.BuyerDepositorRepository;
import app.ewarehouse.repository.BuyerPaymentInvoiceRepository;
import app.ewarehouse.repository.DocumentRepository;
import app.ewarehouse.service.BuyerPaymentInvoiceService;
import app.ewarehouse.util.CommonUtil;
import app.ewarehouse.util.DocumentUploadutil;
import app.ewarehouse.util.FolderAndDirectoryConstant;
import com.fasterxml.jackson.databind.ObjectMapper;
import jakarta.persistence.EntityManager;
import jakarta.persistence.ParameterMode;
import jakarta.persistence.StoredProcedureQuery;
import lombok.RequiredArgsConstructor;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.Base64;
import java.util.List;
import java.util.Optional;
import java.util.UUID;

@Service
@RequiredArgsConstructor
public class BuyerPaymentInvoiceServiceImpl implements BuyerPaymentInvoiceService {

    private final BuyerPaymentInvoiceRepository buyerPaymentInvoiceRepository;
    private final DocumentRepository documentRepository;
    private final EntityManager entityManager;

    @Override
    @Transactional
    public String saveData(String data) {
        try {
            BuyerPaymentInvoiceRequest buyerPaymentInvoiceRequest = new ObjectMapper()
                    .readValue(CommonUtil.inputStreamDecoder(data), BuyerPaymentInvoiceRequest.class);

            byte[] decode = Base64.getDecoder().decode(buyerPaymentInvoiceRequest.getContractAgreement().getBytes());
            String uniqueFileName = "Buyer_Payment_Receipt_" + UUID.randomUUID();
            String file_url = DocumentUploadutil.uploadFileByte(uniqueFileName, decode,FolderAndDirectoryConstant.CONTRACT_AGREEMENT_FOLDER);

            if (file_url.startsWith("Document")) {
                throw new CustomGeneralException(file_url);
            }
         

            String filePath = file_url.substring(file_url.indexOf(FolderAndDirectoryConstant.CONTRACT_AGREEMENT_FOLDER));

            // Save document details
            Document document = new Document();
            document.setDocName(uniqueFileName);
            document.setDocPath(filePath);
            document.setCreatedBy("system");
            documentRepository.save(document);

            BuyerDepositor buyer = new BuyerDepositor();
            buyer.setIntId(buyerPaymentInvoiceRequest.getBuyerId());

            TwarehouseReceipt twarehouseReceipt = new TwarehouseReceipt();
            twarehouseReceipt.setTxtWarehouseReceiptId(buyerPaymentInvoiceRequest.getWareHouseReceiptNo());

            BuyerPaymentInvoice buyerPaymentInvoice = new BuyerPaymentInvoice();
            buyerPaymentInvoice.setInvoiceNumber(generateCustomId("t_buyer_payment_invoice", "invoiceNumber"));
            buyerPaymentInvoice.setContractAgreementFileUrl(filePath);
            buyerPaymentInvoice.setBuyer(buyer);
            buyerPaymentInvoice.setWarehouseReceipt(twarehouseReceipt);
            buyerPaymentInvoice.setPrice(buyerPaymentInvoiceRequest.getPriceOfReceipt());
            buyerPaymentInvoice.setDocument(document);
            
            BuyerPaymentInvoice savedBuyerPaymentInvoice = buyerPaymentInvoiceRepository.save(buyerPaymentInvoice);
            return savedBuyerPaymentInvoice.getInvoiceNumber();
        } catch (Exception e) {
            throw new RuntimeException(e);
        }
    }

    public String generateCustomId(String tableName, String idName) {
        StoredProcedureQuery query = entityManager.createStoredProcedureQuery("GenerateCustomID")
                .registerStoredProcedureParameter(1, String.class, ParameterMode.IN)
                .registerStoredProcedureParameter(2, String.class, ParameterMode.IN)
                .registerStoredProcedureParameter(3, String.class, ParameterMode.OUT)
                .setParameter(1, tableName)
                .setParameter(2, idName);

        query.execute();
        return (String) query.getOutputParameterValue(3);
    }

    @Override
    public Optional<BuyerPaymentInvoice> getInvoiceByNumber(String invoiceNumber) {
    	
    	
        return buyerPaymentInvoiceRepository.findByInvoiceNumber(invoiceNumber);
    }

    @Override
    public Document saveDocument(Document document) {
        return documentRepository.save(document);
    }

}
