package app.ewarehouse.controller;

import app.ewarehouse.entity.BuyerPaymentInvoice;
import app.ewarehouse.service.BuyerPaymentInvoiceService;
import app.ewarehouse.util.CommonUtil;

import app.ewarehouse.util.FileDownloadUtil;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.io.InputStreamResource;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;

import java.io.IOException;
import java.util.List;
import java.util.Optional;

@RestController
@RequestMapping("/api/buyer-payment-invoices")
public class BuyerPaymentInvoiceController {

	@Autowired
	private BuyerPaymentInvoiceService buyerPaymentInvoiceService;
	
	@Autowired
	private ObjectMapper objectMapper;

//	@PostMapping("/save")
//	public String saveData(@RequestBody String data) {
////		return buyerPaymentInvoiceService.saveData(data);
//	}
	
	@PostMapping("/save")
	public ResponseEntity<String> saveData(@RequestBody String data) throws JsonProcessingException {
		return ResponseEntity.ok(CommonUtil.encodedJsonResponse(buyerPaymentInvoiceService.saveData(data), objectMapper));
	}

//	@GetMapping("/buyer/{buyerId}")
//	public List<BuyerPaymentInvoice> getInvoicesByBuyerId(@PathVariable String buyerId) {
//		return buyerPaymentInvoiceService.getInvoicesByBuyerId(buyerId);
//	}

//	@GetMapping("/all")
//	public List<BuyerPaymentInvoice> getAllInvoices() {
//		return buyerPaymentInvoiceService.getAllInvoices();
//	}

//	@GetMapping("/{invoiceNumber}")
//	public Optional<BuyerPaymentInvoice> getInvoiceByNumber(@PathVariable String invoiceNumber) {
//		return buyerPaymentInvoiceService.getInvoiceByNumber(invoiceNumber);
//	}
	
	@GetMapping()
	public ResponseEntity<?> getInvoiceByNumber(@RequestParam String invoiceNumber) throws JsonProcessingException {
		System.out.println(invoiceNumber);
		return ResponseEntity.ok(CommonUtil.encodedJsonResponse(buyerPaymentInvoiceService.getInvoiceByNumber(invoiceNumber),objectMapper));

	}

	@GetMapping("/download")
	public ResponseEntity<InputStreamResource> download(HttpServletResponse response, HttpServletRequest request, @RequestParam("filePath") String filePath) throws IOException {
		String[] parts = filePath.split("/");
		String fileName = parts[parts.length - 1];
		return FileDownloadUtil.fileDownloadUtil(fileName, filePath, response, request);
	}

}

