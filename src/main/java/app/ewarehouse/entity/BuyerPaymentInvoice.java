package app.ewarehouse.entity;

import jakarta.persistence.*;
import lombok.Data;

import org.hibernate.annotations.CreationTimestamp;
import org.hibernate.annotations.UpdateTimestamp;

import java.math.BigDecimal;
import java.time.LocalDateTime;

@Entity
@Table(name = "t_buyer_payment_invoice")
@Data
public class BuyerPaymentInvoice {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer id;

    @Column(name = "invoiceNumber")
    private String invoiceNumber;

    private BigDecimal price;

//    private String depositorId;
//
//    private String documentId;

    @CreationTimestamp
    private LocalDateTime createdAt;
    @UpdateTimestamp
    private LocalDateTime updatedAt;
    
    @ManyToOne
    private BuyerDepositor buyer;

    @OneToOne
    private TwarehouseReceipt warehouseReceipt;

    private String contractAgreementFileUrl;

    //private String signatureFileUrl; // New field for signature


    @OneToOne
    @JoinColumn(name = "document_id", referencedColumnName = "docID")
    private Document document;

}

