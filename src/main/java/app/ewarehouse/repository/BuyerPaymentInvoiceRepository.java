package app.ewarehouse.repository;

import app.ewarehouse.entity.BuyerPaymentInvoice;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;


@Repository
public interface BuyerPaymentInvoiceRepository extends JpaRepository<BuyerPaymentInvoice, Integer> {

    Optional<BuyerPaymentInvoice> findByInvoiceNumber(String invoiceNumber);

}
