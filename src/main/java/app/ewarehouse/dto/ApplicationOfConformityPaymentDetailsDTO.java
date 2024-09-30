package app.ewarehouse.dto;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@NoArgsConstructor
@AllArgsConstructor
public class ApplicationOfConformityPaymentDetailsDTO {

	private Integer userId;
    private String paymentType;
    private Integer intParticularOfApplicantsId;
	
}
