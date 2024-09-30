package app.ewarehouse.controller;

import app.ewarehouse.entity.Commodity;
import app.ewarehouse.service.CommodityService;
import app.ewarehouse.util.CommonUtil;
import app.ewarehouse.util.ErrorMessages;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/commodities")
@CrossOrigin("*")
public class CommodityController {
	
	@Autowired
	private CommodityService commodityService;
	@Autowired
	ObjectMapper objectMapper;
	@Autowired
	ErrorMessages errorMessages;
	
	@PostMapping
	public ResponseEntity<?> createCommodity(@RequestBody String commodityRequest) throws JsonProcessingException {
        try {
			Integer isSaved = commodityService.save(commodityRequest);
			return ResponseEntity.ok(CommonUtil.encodedJsonResponse(isSaved,objectMapper));
        } catch (Exception e){
			e.printStackTrace();
			return ResponseEntity.ok(1);
        }
    }
	    @GetMapping
	    public ResponseEntity<?> getAllCommodities() throws JsonProcessingException {
			List<Commodity> commodities = commodityService.getAll();
			return ResponseEntity.ok(CommonUtil.encodedJsonResponse(commodities,objectMapper));
	    }
	@GetMapping("/paginated")
	public ResponseEntity<?> getAllCommoditiesPageable(
			@RequestParam(defaultValue = "0") int page,
			@RequestParam(defaultValue = "10") int size) throws JsonProcessingException {
			Pageable pageable = PageRequest.of(page, size);
			Page<Commodity> pageResult = commodityService.getAllCommoditiesList(pageable);
			return ResponseEntity.ok(CommonUtil.encodedJsonResponse(pageResult,objectMapper));
	}

	    @GetMapping("/{id}")
	    public ResponseEntity<?> getCommodityById(@PathVariable Integer id) throws JsonProcessingException {
			Commodity commodity = commodityService.getById(id);
			if (commodity != null) {
				return ResponseEntity.ok(CommonUtil.encodedJsonResponse(commodity,objectMapper));
			} else {
				return new ResponseEntity<>(HttpStatus.NOT_FOUND);
			}
	    }

	    @PutMapping("/{id}")
	    public ResponseEntity<?> updateCommodity(@PathVariable Integer id, @RequestBody String updatedCommodity) throws JsonProcessingException {
			 Integer isUpdated = commodityService.update(id, updatedCommodity);
				return ResponseEntity.ok(CommonUtil.encodedJsonResponse(isUpdated,objectMapper));
	    }

    @DeleteMapping("/{id}")
    public ResponseEntity<?> deleteCommodity(@PathVariable Integer id) throws Exception {
			boolean isDeleted = commodityService.delete(id);
		return ResponseEntity.ok(Map.of("status", "DELETED", "msg", "Request successful"));
//			ResponseDTO<Object> responseDTO;
//			if (isDeleted) {
//				responseDTO = ResponseDTO.builder()
//						.status(HttpStatus.NO_CONTENT.value())
//						.message("Commodity deactivated successfully")
//						.build();
//				return ResponseEntity.ok(CommonUtil.encodedJsonResponse(responseDTO,objectMapper));
//			} else {
//				responseDTO = ResponseDTO.builder()
//						.status(HttpStatus.INTERNAL_SERVER_ERROR.value())
//						.message("Failed to delete commodity")
//						.build();
//
//				return ResponseEntity.ok(responseDTO);
//			}
    }
}
