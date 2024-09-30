package app.ewarehouse.master.service;

import app.ewarehouse.master.dto.ComplaintStatusMasterDto;
import app.ewarehouse.master.entity.ComplaintStatusMaster;

import java.util.List;

import org.json.JSONObject;
/**
 * @author chinmaya.jena
 * @since 03-07-2024
 */
public interface ComplaintStatusMasterService {

	ComplaintStatusMaster createComplaintStatus(ComplaintStatusMasterDto complaintStatusMasterDto);
    ComplaintStatusMaster updateComplaintStatus(Integer id, ComplaintStatusMasterDto complaintStatusMasterDto);
    ComplaintStatusMaster getComplaintStatusById(Integer id);
    List<ComplaintStatusMaster> getAllComplaintStatuses();
    JSONObject changeComplaintStatus(Integer id);
	
}
