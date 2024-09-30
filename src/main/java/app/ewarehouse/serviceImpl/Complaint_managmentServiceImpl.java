package app.ewarehouse.serviceImpl;

import java.io.File;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.StandardCopyOption;
import java.text.SimpleDateFormat;
import java.time.format.DateTimeFormatter;
import java.util.*;
import java.util.concurrent.atomic.AtomicInteger;
import java.util.stream.Collectors;

import app.ewarehouse.dto.ComplaintDetailsComprehensive;
import app.ewarehouse.entity.*;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.core.type.TypeReference;
import org.json.JSONArray;
import org.json.JSONObject;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageImpl;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;
import org.springframework.stereotype.Service;

import com.fasterxml.jackson.databind.DeserializationFeature;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.google.common.base.Strings;

import app.ewarehouse.dto.ComplaintmanagementResponse;
import app.ewarehouse.exception.CustomGeneralException;
import app.ewarehouse.repository.ApplicationOfConformityRepository;
import app.ewarehouse.repository.ComplaintMgmtInspScheduleDocsRepository;
import app.ewarehouse.repository.ComplaintMgmtInspScheduleRepository;
import app.ewarehouse.repository.ComplaintObservationRepository;
import app.ewarehouse.repository.ComplaintObservationResponseRepository;
import app.ewarehouse.repository.ComplaintTypeRepository;
import app.ewarehouse.repository.Complaint_managmentRepository;
import app.ewarehouse.repository.CountyRepository;
import app.ewarehouse.repository.OnlineServiceApprovalNotingsRepository;
import app.ewarehouse.repository.SubCountyRepository;
import app.ewarehouse.repository.Supporting_documentRepository;
import app.ewarehouse.repository.TOnlineServiceApplicationHRepository;
import app.ewarehouse.repository.TOnlineServiceApprovalRepository;
import app.ewarehouse.repository.TSetAuthorityRepository;
import app.ewarehouse.repository.TapplicationOfCertificateOfComplianceRepository;
import app.ewarehouse.repository.TmenulinksRepository;
import app.ewarehouse.repository.TuserRepository;
import app.ewarehouse.service.Complaint_managmentService;
import app.ewarehouse.util.DocumentUploadutil;
import app.ewarehouse.util.ErrorMessages;
import app.ewarehouse.util.FnAuthority;
import app.ewarehouse.util.FolderAndDirectoryConstant;
import app.ewarehouse.util.Mapper;
import jakarta.persistence.EntityManager;
import jakarta.transaction.Transactional;

@Transactional
@Service
public class Complaint_managmentServiceImpl implements Complaint_managmentService {
	@Autowired
	private Complaint_managmentRepository complaint_managmentRepository;
	@Autowired
    TapplicationOfCertificateOfComplianceRepository repo;
	@Autowired
	EntityManager entityManager;
	@Autowired
	private ComplaintObservationResponseRepository complaintObservationResponseRepository;
	@Autowired
	private  ApplicationOfConformityRepository applicationOfConformityRepository;

	@Autowired
	ErrorMessages errorMessages;

	@Autowired
	ComplaintObservationRepository observationRepo;
	@Autowired
	TOnlineServiceApprovalRepository tOnlineServiceApprovalRepository;
	@Autowired
	private TOnlineServiceApplicationHRepository tOnlineServiceApplicationHRepository;
	@Autowired
	TmenulinksRepository tmenulinksRepository;
	@Autowired
	TSetAuthorityRepository tSetAuthorityRepository;
	@Autowired
	private FnAuthority fnAuthority;
	@Autowired
	private TuserRepository tuserRepository;

	@Autowired
	private CountyRepository countyRepo;

	@Autowired
	private SubCountyRepository subCountyRepo;

	@Autowired
	private ApplicationOfConformityRepository appRepo;

	@Autowired
	private ComplaintTypeRepository ctRepo;

	@Autowired
	private ComplaintMgmtInspScheduleDocsRepository docRepo;

	@Autowired
	private OnlineServiceApprovalNotingsRepository onlineServiceApprovalNotingsRepository;

	@Autowired
	private ComplaintObservationResponseRepository resRepo;

	@Autowired
	private ComplaintMgmtInspScheduleRepository compMgmtRepo;
	public static final String PENDING_AUTHORITIES = "pendingAuthorities";
	public static final String STAGENO = "stageNo";
	public static final String ATAPROCESSID = "intATAProcessId";

	@Autowired
	private Supporting_documentRepository supporting_documentRepository;
	@Autowired
	WorkflowServiceImpl workflowServiceImpl;

	JSONObject selCounty = new JSONObject("{'1':'County1','2':'County2','3':'County3'}");
	JSONObject selSubCounty = new JSONObject("{'1':'Sub County1','2':'Sub County2','3':'Sub County3'}");
	JSONObject rdoComplaintAgainst = new JSONObject(
			"{'101':'Warehouse Operator','102':'Inspector','103':'Collateral Manager','104':'Grader'}");
	JSONObject selCountyofWarehouse = new JSONObject(
			"{'1':'County of Warehouse1','2':'County of Warehouse2','3':'County of Warehouse3'}");
	JSONObject selSubCountyofWarehouse = new JSONObject(
			"{'1':'Sub-County of Warehouse1','2':'Sub-County of Warehouse2','3':'Sub-County of Warehouse3'}");
	JSONObject selWarehouseShopName = new JSONObject(
			"{'1':'Warehouse Shop Name1','2':'Warehouse Shop Name2','3':'Warehouse Shop Name3'}");
	JSONObject selNameofGrader = new JSONObject(
			"{'1':' Name of Grader1','2':' Name of Grader2','3':' Name of Grader3'}");
	JSONObject selNameofCollateralManager = new JSONObject(
			"{'1':'Name of Collateral Manager1','2':'Name of Collateral Manager2','3':'Name of Collateral Manager3'}");
	JSONObject selNameofInspector = new JSONObject(
			"{'1':'Name of Inspector1','2':'Name of Inspector2','3':'Name of Inspector3'}");
	JSONObject selTypeofComplain = new JSONObject(
			"{'1':'Type of Complain1','2':'Type of Complain2','3':'Type of Complain3'}");

	Integer parentId = 0;
	Object dynamicValue = null;
	private static final Logger logger = LoggerFactory.getLogger(Complaint_managmentServiceImpl.class);
	JSONObject json = new JSONObject();
	@Value("${tempUpload.path}")
	private String tempUploadPath;
	@Value("${finalUpload.path}")
	private String finalUploadPath;

	@Override
	public JSONObject save(String data) throws Exception {
		Complaint_managment saveData = null;
		logger.info("Inside save method of Complaint_managmentServiceImpl");
		ObjectMapper om = new ObjectMapper();
		om.configure(DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES, false);
		Complaint_managment complaint_managment = om.readValue(data, Complaint_managment.class);
		List<String> fileUploadList = new ArrayList<String>();
		if (!Objects.isNull(complaint_managment.getIntId()) && complaint_managment.getIntId() > 0) {
			Complaint_managment getEntity = complaint_managmentRepository.findByIntIdAndBitDeletedFlag(complaint_managment.getIntId(), false);
			getEntity.setTxtFullName(complaint_managment.getTxtFullName());
			getEntity.setTxtContactNumber(complaint_managment.getTxtContactNumber());
			getEntity.setTxtEmailAddress(complaint_managment.getTxtEmailAddress());
			getEntity.setTxtrAddress(complaint_managment.getTxtrAddress());
			getEntity.setSelCounty(complaint_managment.getSelCounty());
			getEntity.setSelSubCounty(complaint_managment.getSelSubCounty());
			getEntity.setRdoComplaintAgainst(complaint_managment.getRdoComplaintAgainst());
			getEntity.setSelCountyofWarehouse(complaint_managment.getSelCountyofWarehouse());
			getEntity.setSelSubCountyofWarehouse(complaint_managment.getSelSubCountyofWarehouse());
			getEntity.setSelWarehouseShopName(complaint_managment.getSelWarehouseShopName());
			getEntity.setTxtWarehouseOperator(complaint_managment.getTxtWarehouseOperator());
			getEntity.setSelNameofGrader(complaint_managment.getSelNameofGrader());
			getEntity.setSelNameofCollateralManager(complaint_managment.getSelNameofCollateralManager());
			getEntity.setSelNameofInspector(complaint_managment.getSelNameofInspector());
			getEntity.setSelTypeofComplain(complaint_managment.getSelTypeofComplain());
			getEntity.setTxtDateofIncident(complaint_managment.getTxtDateofIncident());
			getEntity.setTxtrDescriptionofIncident(complaint_managment.getTxtrDescriptionofIncident());
			getEntity.setChkdeclartion(complaint_managment.getChkdeclartion());
			getEntity.setVchActionOnApplication("noAction");
			getEntity.setActionCondition("noCondition");
			getEntity.setComplaintNo(complaint_managment.getComplaintNo());
			Complaint_managment updateData = complaint_managmentRepository.save(getEntity);

			parentId = updateData.getIntId();
			json.put("status", 202);
			supporting_documentRepository.deleteAllByIntParentId(parentId);
		} else {
			complaint_managment.setVchActionOnApplication("noAction");
			complaint_managment.setActionCondition("noCondition");
			complaint_managment.setComplaintNo(getComplaintNo("COM"));
			saveData = complaint_managmentRepository.save(complaint_managment);
			parentId = saveData.getIntId();
			json.put("complaintNo", saveData.getComplaintNo());
			json.put("status", 200);
		}
		List<Supporting_document> supporting_documentList = complaint_managment.getAddMoreSupportingDocuments();
		supporting_documentList.forEach(t -> {
			t.setIntParentId(parentId);
			fileUploadList.add(t.getAmfileUploadDocuments());
		});
		supporting_documentRepository.saveAll(supporting_documentList);
		fileUploadList.forEach(fileUpload -> {
			if (!Strings.isNullOrEmpty(fileUpload)) {
				File f = new File(tempUploadPath + fileUpload);
				if (f.exists()) {
					File src = new File(tempUploadPath + fileUpload);
					File dest = new File(finalUploadPath + "complaint-management/" + fileUpload);
					try {
						Files.copy(src.toPath(), dest.toPath(), StandardCopyOption.REPLACE_EXISTING);
						Files.delete(src.toPath());
					} catch (IOException e) {
						logger.error(
								"Inside save method of Complaint_managmentServiceImpl some error occur in file moving to destination folder:"
										+ e);
					}
				}
			}
		});// FOR WORKFLOW

		int complaint_aginast_id = saveData.getRdoComplaintAgainst();
		String complaintTypeId = saveData.getSelTypeofComplain();
		Integer intLabelId = 0;

		if (complaint_aginast_id == 101) {
			if (complaintTypeId.equals("8") || complaintTypeId.equals("9") || complaintTypeId.equals("10")
					|| complaintTypeId.equals("11")) {
				intLabelId = 101;
			} else {
				intLabelId = 103;
			}

		} else if (complaint_aginast_id == 102) {
			intLabelId = 102;
		} else if (complaint_aginast_id == 103) {
			intLabelId = 104;
		} else if (complaint_aginast_id == 104) {
			intLabelId = 103;
		}

		Integer processId = 160;

		JSONObject tsetAuthority = fnAuthority.getAuthority(processId, 1, intLabelId);
		if (tsetAuthority.length() > 0) {

			String pendingAt = tsetAuthority.getString(PENDING_AUTHORITIES);
			int stageNo = tsetAuthority.getInt(STAGENO);
			int intATAProcessId = tsetAuthority.getInt(ATAPROCESSID);
			TOnlineServiceApproval tOnlineServiceApprovalDelete = tOnlineServiceApprovalRepository
					.getByIntOnlineServiceIdAndIntProcessIdAndBitDeletedFlag(parentId, processId, false);
			if (tOnlineServiceApprovalDelete != null) {
				tOnlineServiceApprovalDelete.setBitDeletedFlag(true);
				tOnlineServiceApprovalRepository.save(tOnlineServiceApprovalDelete);
			}
			TOnlineServiceApproval tOnlineServiceApproval = new TOnlineServiceApproval();
			tOnlineServiceApproval.setIntOnlineServiceId(parentId);
			tOnlineServiceApproval.setIntStageNo(stageNo);
			tOnlineServiceApproval.setIntProcessId(processId);
			tOnlineServiceApproval.setIntPendingAt(pendingAt);
			tOnlineServiceApproval.setIntATAProcessId(intATAProcessId);
			tOnlineServiceApproval.setIntLabelId(intLabelId);
			tOnlineServiceApprovalRepository.save(tOnlineServiceApproval);
		}
		json.put("id", parentId);
		return json;
	}

	private String getComplaintNo(String id) {
		String dbCurrentId = complaint_managmentRepository.getId();
		if (dbCurrentId == null) {
			return id + "100000";
		} else {
			Integer idNum = Integer.parseInt(dbCurrentId.substring(3, dbCurrentId.length()));
			AtomicInteger seq = new AtomicInteger(idNum);
			int nextVal = seq.incrementAndGet();
			return id + nextVal;
		}
	}

	@Override
	public JSONObject getById(Integer id) throws Exception {
		logger.info("Inside getById method of Complaint_managmentServiceImpl");
		Complaint_managment entity = complaint_managmentRepository.findByIntIdAndBitDeletedFlag(id, false);

		dynamicValue = (!Objects.isNull(entity.getSelCounty()) && selCounty.has(entity.getSelCounty().toString()))
				? selCounty.get(entity.getSelCounty().toString())
				: "--";
		County county = countyRepo.findById(entity.getSelCounty()).get();
		entity.setSelCountyVal(county.getName());
		dynamicValue = (!Objects.isNull(entity.getSelSubCounty())
				&& selSubCounty.has(entity.getSelSubCounty().toString()))
						? selSubCounty.get(entity.getSelSubCounty().toString())
						: "--";
		SubCounty subCounty = subCountyRepo.findById(entity.getSelSubCounty()).get();
		entity.setSelSubCountyVal(subCounty.getTxtSubCountyName());
		dynamicValue = (!Objects.isNull(entity.getRdoComplaintAgainst())
				&& rdoComplaintAgainst.has(entity.getRdoComplaintAgainst().toString()))
						? rdoComplaintAgainst.get(entity.getRdoComplaintAgainst().toString())
						: "--";
		entity.setRdoComplaintAgainstVal((String) dynamicValue);
		dynamicValue = (!Objects.isNull(entity.getSelCountyofWarehouse())
				&& selCountyofWarehouse.has(entity.getSelCountyofWarehouse().toString()))
						? selCountyofWarehouse.get(entity.getSelCountyofWarehouse().toString())
						: "--";
		if (!entity.getSelCountyofWarehouse().toString().equals("0")) {
			County selcounty = countyRepo.findById(entity.getSelCountyofWarehouse()).get();
			entity.setSelCountyofWarehouseVal(selcounty.getName());
		}

		dynamicValue = (selSubCountyofWarehouse.has(entity.getSelSubCountyofWarehouse().toString()))
				? selSubCountyofWarehouse.get(entity.getSelSubCountyofWarehouse().toString())
				: "--";
		if (!entity.getSelSubCountyofWarehouse().toString().equals("0")) {
			SubCounty selsubCounty = subCountyRepo.findById(entity.getSelSubCountyofWarehouse()).get();
			entity.setSelSubCountyofWarehouseVal(selsubCounty.getTxtSubCountyName());
		}

		dynamicValue = (selWarehouseShopName.has(entity.getSelWarehouseShopName().toString()))
				? selWarehouseShopName.get(entity.getSelWarehouseShopName().toString())
				: "--";
		if (!entity.getSelWarehouseShopName().toString().equals("0")) {
			ApplicationOfConformity app = appRepo.findById(entity.getSelWarehouseShopName()).get();
			entity.setSelWarehouseShopNameVal(app.getParticularOfApplicantsId().getShop());
		}
		dynamicValue = (!Objects.isNull(entity.getSelNameofGrader())
				&& selNameofGrader.has(entity.getSelNameofGrader().toString()))
						? selNameofGrader.get(entity.getSelNameofGrader().toString())
						: "--";
		if (!entity.getSelNameofGrader().toString().equals("0")) {
			System.out.println(entity.getSelNameofGrader());
			Tuser cm = tuserRepository.findById(Integer.parseInt(entity.getSelNameofGrader())).get();
			entity.setSelNameofGraderVal(cm.getTxtFullName());
		}

		dynamicValue = (!Objects.isNull(entity.getSelNameofCollateralManager())
				&& selNameofCollateralManager.has(entity.getSelNameofCollateralManager().toString()))
						? selNameofCollateralManager.get(entity.getSelNameofCollateralManager().toString())
						: "--";
		if (!entity.getSelNameofCollateralManager().toString().equals("0")) {
			Tuser cm = tuserRepository.findById(Integer.parseInt(entity.getSelNameofCollateralManager())).get();
			entity.setSelNameofCollateralManagerVal(cm.getTxtFullName());
		}

		dynamicValue = (!Objects.isNull(entity.getSelNameofInspector())
				&& selNameofInspector.has(entity.getSelNameofInspector().toString()))
						? selNameofInspector.get(entity.getSelNameofInspector().toString())
						: "--";
		if (!entity.getSelNameofInspector().toString().equals("0")) {
			Tuser insp = tuserRepository.findById(Integer.parseInt(entity.getSelNameofInspector())).get();
			entity.setSelNameofInspectorVal(insp.getTxtFullName());
		}

		dynamicValue = (!Objects.isNull(entity.getSelTypeofComplain())
				&& selTypeofComplain.has(entity.getSelTypeofComplain().toString()))
						? selTypeofComplain.get(entity.getSelTypeofComplain().toString())
						: "--";
		ComplaintType ct = ctRepo.findById(Integer.parseInt(entity.getSelTypeofComplain())).get();
		entity.setSelTypeofComplainVal(ct.getComplaintType());
		List<Supporting_document> supporting_documentList = supporting_documentRepository
				.findByIntParentIdAndBitDeletedFlag(id, false);
		entity.setAddMoreSupportingDocuments(supporting_documentList);

		return new JSONObject(entity);
	}

	@Override
	public JSONObject getAll(String formParams) throws Exception {
		logger.info("Inside getAll method of Complaint_managmentServiceImpl");
		JSONObject jsonData = new JSONObject(formParams);
		String txtFullName = "0";
		String txtContactNumber = "0";
		Integer intCreatedBy = 0;
		Integer totalDataPresent = 0;
		if (jsonData.has("txtFullName") && !jsonData.isNull("txtFullName")
				&& !jsonData.getString("txtFullName").equals("")) {
			txtFullName = jsonData.getString("txtFullName");
		}
		if (jsonData.has("txtContactNumber") && !jsonData.isNull("txtContactNumber")
				&& !jsonData.getString("txtContactNumber").equals("")) {
			txtContactNumber = jsonData.getString("txtContactNumber");
		}
		
		if (jsonData.has("intCreatedBy") && !jsonData.isNull("intCreatedBy")
				&& !jsonData.getString("intCreatedBy").equals("")) {
			intCreatedBy = jsonData.getInt("intCreatedBy");
		}
		if(intCreatedBy == 0) {
		totalDataPresent = complaint_managmentRepository.countByBitDeletedFlagAndIntCreatedBy(false , 0);
		}else {
			totalDataPresent = complaint_managmentRepository.countByBitDeletedFlagAndIntCreatedBy(false , intCreatedBy);
		}
		Pageable pageRequest = PageRequest.of(jsonData.has("pageNo") ? jsonData.getInt("pageNo") - 1 : 0,
				jsonData.has("size") ? jsonData.getInt("size") : totalDataPresent,
				Sort.by(Sort.Direction.DESC, "intId"));
		List<Complaint_managment> complaint_managmentResp = complaint_managmentRepository
				.findAllByBitDeletedFlagAndIntInsertStatus(false, pageRequest, txtFullName, txtContactNumber , intCreatedBy);
		complaint_managmentResp.forEach(entity -> {
			Integer count = onlineServiceApprovalNotingsRepository.getCountedData(12, entity.getIntId());
			entity.setNotingCount(count);
			TOnlineServiceApproval approval = tOnlineServiceApprovalRepository
					.findByIntOnlineServiceIdAndIntStageNoAndBitDeletedFlag(160, entity.getIntId(), false);
			if (approval != null) {
				entity.setTinStatus(approval.getTinStatus());
				entity.setIntProcessId(approval.getIntProcessId());
				entity.setResubmitCount(onlineServiceApprovalNotingsRepository
						.countByIntOnlineServiceIdAndIntProcessIdAndIntStatusAndBitDeletedFlag(entity.getIntId(),
								approval.getIntProcessId(), 3, false));

				String cm = tuserRepository.getPendignAtUser(Integer.parseInt(approval.getIntPendingAt()));
				entity.setPendingATUser(cm);

				// entity.setForwardedToUser(approval.getIntForwardTo());
				// entity.setSentFromUser(approval.getIntSentFrom().toString());
			}
			dynamicValue = (!Objects.isNull(entity.getSelCounty()) && selCounty.has(entity.getSelCounty().toString()))
					? selCounty.get(entity.getSelCounty().toString())
					: "--";
			entity.setSelCountyVal((String) dynamicValue);
			dynamicValue = (!Objects.isNull(entity.getSelSubCounty())
					&& selSubCounty.has(entity.getSelSubCounty().toString()))
							? selSubCounty.get(entity.getSelSubCounty().toString())
							: "--";
			entity.setSelSubCountyVal((String) dynamicValue);
			dynamicValue = (!Objects.isNull(entity.getRdoComplaintAgainst())
					&& rdoComplaintAgainst.has(entity.getRdoComplaintAgainst().toString()))
							? rdoComplaintAgainst.get(entity.getRdoComplaintAgainst().toString())
							: "--";
			entity.setRdoComplaintAgainstVal((String) dynamicValue);
			dynamicValue = (!Objects.isNull(entity.getSelCountyofWarehouse())
					&& selCountyofWarehouse.has(entity.getSelCountyofWarehouse().toString()))
							? selCountyofWarehouse.get(entity.getSelCountyofWarehouse().toString())
							: "--";
			if (!entity.getSelCountyofWarehouse().toString().equals("0")) {
				County selcounty = countyRepo.findById(entity.getSelCountyofWarehouse()).get();
				entity.setSelCountyofWarehouseVal(selcounty.getName());
			}

			dynamicValue = (selSubCountyofWarehouse.has(entity.getSelSubCountyofWarehouse().toString()))
					? selSubCountyofWarehouse.get(entity.getSelSubCountyofWarehouse().toString())
					: "--";
			if (!entity.getSelSubCountyofWarehouse().toString().equals("0")) {
				SubCounty selsubCounty = subCountyRepo.findById(entity.getSelSubCountyofWarehouse()).get();
				entity.setSelSubCountyofWarehouseVal(selsubCounty.getTxtSubCountyName());
			}

			dynamicValue = (selWarehouseShopName.has(entity.getSelWarehouseShopName().toString()))
					? selWarehouseShopName.get(entity.getSelWarehouseShopName().toString())
					: "--";
			if (!entity.getSelWarehouseShopName().toString().equals("0")) {
				ApplicationOfConformity app = appRepo.findById(entity.getSelWarehouseShopName()).get();
				entity.setSelWarehouseShopNameVal(app.getParticularOfApplicantsId().getShop());
			}
			dynamicValue = (!Objects.isNull(entity.getSelNameofGrader())
					&& selNameofGrader.has(entity.getSelNameofGrader().toString()))
							? selNameofGrader.get(entity.getSelNameofGrader().toString())
							: "--";
			if (!entity.getSelNameofGrader().toString().equals("0")) {
				System.out.println(entity.getSelNameofGrader());
				Tuser cm = tuserRepository.findById(Integer.parseInt(entity.getSelNameofGrader())).get();
				entity.setSelNameofGraderVal(cm.getTxtFullName());
			}

			dynamicValue = (!Objects.isNull(entity.getSelNameofCollateralManager())
					&& selNameofCollateralManager.has(entity.getSelNameofCollateralManager().toString()))
							? selNameofCollateralManager.get(entity.getSelNameofCollateralManager().toString())
							: "--";
			if (!entity.getSelNameofCollateralManager().toString().equals("0")) {
				Tuser cm = tuserRepository.findById(Integer.parseInt(entity.getSelNameofCollateralManager())).get();
				entity.setSelNameofCollateralManagerVal(cm.getTxtFullName());
			}

			dynamicValue = (!Objects.isNull(entity.getSelNameofInspector())
					&& selNameofInspector.has(entity.getSelNameofInspector().toString()))
							? selNameofInspector.get(entity.getSelNameofInspector().toString())
							: "--";
			if (!entity.getSelNameofInspector().toString().equals("0")) {
				Tuser insp = tuserRepository.findById(Integer.parseInt(entity.getSelNameofInspector())).get();
				entity.setSelNameofInspectorVal(insp.getTxtFullName());
			}

			dynamicValue = (!Objects.isNull(entity.getSelTypeofComplain())
					&& selTypeofComplain.has(entity.getSelTypeofComplain().toString()))
							? selTypeofComplain.get(entity.getSelTypeofComplain().toString())
							: "--";
			if (!entity.getSelTypeofComplain().toString().equals("0")) {
				ComplaintType ct = ctRepo.findById(Integer.parseInt(entity.getSelTypeofComplain())).get();
				entity.setSelTypeofComplainVal(ct.getComplaintType());
			} else {
				entity.setSelTypeofComplainVal("--");
			}
			if (entity.getComplaintNo().isEmpty()) {
				entity.setComplaintNo("--");
			}
			if (entity.getRdoComplaintAgainst()==0) {
				entity.setComplaintNo("--");
			}
			
			
			// entity.setSelTypeofComplainVal((String) dynamicValue);

		});
		json.put("status", 200);
		json.put("result", new JSONArray(complaint_managmentResp));
		json.put("count", totalDataPresent);
		logger.info("json contians:" + json);
		return json;
	}

	@Override
	public JSONObject getByActionWise(String data) throws Exception {
		logger.info("Inside getByActionWise method of Complaint_managmentServiceImpl");
		JSONObject jsonObject = new JSONObject(data);
		JSONArray jsonArray = new JSONArray();
		Integer intId = jsonObject.getInt("intId");
		Integer roleId = jsonObject.getInt("roleId");
		Integer userId = jsonObject.getInt("userId");
		JSONObject jsonData = jsonObject.getJSONObject("formData");
		String txtFullName = "0";
		String txtContactNumber = "0";
		if (jsonData.has("txtFullName") && !jsonData.isNull("txtFullName")
				&& !jsonData.getString("txtFullName").equals("")) {
			txtFullName = jsonData.getString("txtFullName");
		}
		if (jsonData.has("txtContactNumber") && !jsonData.isNull("txtContactNumber")
				&& !jsonData.getString("txtContactNumber").equals("")) {
			txtContactNumber = jsonData.getString("txtContactNumber");
		}

		String applicationStatus = jsonObject.getString("applicationStatus");
		String applicationType = jsonObject.optString("applicationType");

		String vchAuths = tSetAuthorityRepository.getVchAuthsByProcessIdAndRoleIdup(intId, roleId);
		boolean isPresent = false;
		boolean isGenerate = false;
		if (vchAuths != null) {
			String[] vchAuthList = vchAuths.split(",");
			for (String num : vchAuthList) {
				int currentNumber = Integer.parseInt(num.trim());
				if (currentNumber == 13) {
					isPresent = true;

				}
				if (currentNumber == 14) {
					isGenerate = true;
				}
			}
		}
		if (isPresent && isGenerate) {
			json.put("draftStatus", 1);
		} else if (isGenerate) {
			json.put("draftStatus", 1);
		} else {
			json.put("draftStatus", 0);
		}

		Page<Map<String, Object>> obj = null;
		Pageable pageRequest = PageRequest.of(jsonObject.has("pageNo") ? jsonObject.getInt("pageNo")-1 : 0, jsonObject.optInt("size"));
		
		if (applicationStatus.equals("pending")) {
			if (applicationType.equals("review")) {
				if (roleId == 3) {
					Integer isInspAssign = complaint_managmentRepository.checkInspectorAssign(userId, roleId);
					if (isInspAssign > 0) {
		             obj = complaint_managmentRepository.getAllInspectorPendingData(intId, roleId, userId,txtFullName, txtContactNumber, pageRequest);

					}else {
						obj = Page.empty();

					}
				} else {
					obj = complaint_managmentRepository.getAllReviewData(intId, roleId, userId, txtFullName,txtContactNumber, pageRequest);
				}
			} else if (applicationType.equals("assign")) {

				obj = complaint_managmentRepository.getAllAssignData(intId, roleId, userId, txtFullName,
						txtContactNumber, pageRequest);
			} else {
				obj = complaint_managmentRepository.getAllPendingData(intId, roleId, userId, txtFullName,
						txtContactNumber, pageRequest);
			}
		} else if (applicationStatus.equals("approved")) {
			if ("".equals(applicationType))
				obj = complaint_managmentRepository.getAllApprovedData(intId, txtFullName, txtContactNumber, pageRequest);
			else
				obj = complaint_managmentRepository.getAllApprovedDataNew(intId, applicationType, txtFullName, txtContactNumber, pageRequest);
		} else if (applicationStatus.equals("reject")) {
			if ("".equals(applicationType))
				obj = complaint_managmentRepository.getAllRejectedData(intId, txtFullName, txtContactNumber, pageRequest);
			else
				obj = complaint_managmentRepository.getAllRejectedDataNew(intId, applicationType, txtFullName, txtContactNumber, pageRequest);
		} else if (applicationStatus.equals("summary")) {
			//obj = complaint_managmentRepository.getAllSummaryData(intId, txtFullName, txtContactNumber,roleId ,pageRequest);
			
			obj = complaint_managmentRepository.getAllSummaryData(intId, txtFullName, txtContactNumber,pageRequest);
		}

		List<Complaint_managment> complaint_managmentList = new ArrayList<>();
		if (!obj.isEmpty()) {
			for (Map<String, Object> obdata : obj) {
				JSONObject jsonOb = new JSONObject();
				jsonOb.put("intId", (Integer) obdata.get("intId"));
				jsonOb.put("dtmCreatedOn", obdata.get("dtmCreatedOn"));
				jsonOb.put("dtmStatusDate",
						new SimpleDateFormat("dd MMM yyyy ,hh.mm a").format((Date) obdata.get("dtmStatusDate")));
				jsonOb.put("tinStatus", (Integer) obdata.get("tinStatus"));
				if ((Integer) obdata.get("tinStatus") == 4) {
					jsonOb.put("userName", obdata.get("userName").toString());
				}
//				if (obdata.get("intPendingAt") != null) {
//					jsonOb.put("intPendingAt", obdata.get("vchRolename"));
//				}
				if (obdata.get("intPendingAt") != null) {
					jsonOb.put("intPendingAt",tuserRepository.getPendignAtUser(Integer.parseInt((String) obdata.get("intPendingAt"))));
				}
				jsonOb.put("stageno", (Integer) obdata.get("intStageNo"));
				jsonOb.put("intLableId", (Integer) obdata.get("intLabelId"));
				jsonOb.put("vchActionOnApplication", obdata.get("vchActionOnApplication"));
				jsonOb.put("ActionCondition", obdata.get("ActionCondition"));
				if (applicationStatus.equals("pending")) {
					Integer tinVerifiedBy = Integer.parseInt(obdata.get("tinVerifiedBy").toString());
					boolean isAction = false;
					if (roleId == tinVerifiedBy) {
						isAction = true;
					}
					if (isAction) {
						jsonOb.put("action", 0);
					} else {
						jsonOb.put("action", 1);
					}
				}
				Integer tinVerifyLetterGenStatus = (Integer) obdata.get("tinVerifyLetterGenStatus");
				jsonOb.put("tinVerifyLetterGenStatus", tinVerifyLetterGenStatus);
				if (tinVerifyLetterGenStatus == 1) {
					jsonOb.put("vchApprovalDoc", obdata.get("vchApprovalDoc").toString());
					JSONArray jsArr = new JSONArray(obdata.get("vchApprovalDoc").toString());
					jsonOb.put("verifyLetter", jsArr);
				}
				Complaint_managment complaint_managment = complaint_managmentRepository
						.findByIntIdAndBitDeletedFlag((Integer) obdata.get("intId"), false);
				dynamicValue = (selCounty.has(complaint_managment.getSelCounty().toString()))
						? selCounty.get(complaint_managment.getSelCounty().toString())
						: "--";
				complaint_managment.setSelCountyVal(dynamicValue.toString());
				dynamicValue = (selSubCounty.has(complaint_managment.getSelSubCounty().toString()))
						? selSubCounty.get(complaint_managment.getSelSubCounty().toString())
						: "--";
				complaint_managment.setSelSubCountyVal(dynamicValue.toString());
				dynamicValue = (rdoComplaintAgainst.has(complaint_managment.getRdoComplaintAgainst().toString()))
						? rdoComplaintAgainst.get(complaint_managment.getRdoComplaintAgainst().toString())
						: "--";
				complaint_managment.setRdoComplaintAgainstVal(dynamicValue.toString());
				dynamicValue = (selCountyofWarehouse.has(complaint_managment.getSelCountyofWarehouse().toString()))
						? selCountyofWarehouse.get(complaint_managment.getSelCountyofWarehouse().toString())
						: "--";

				complaint_managment.setSelCountyofWarehouseVal(dynamicValue.toString());

				dynamicValue = (selSubCountyofWarehouse
						.has(complaint_managment.getSelSubCountyofWarehouse().toString()))
								? selSubCountyofWarehouse
										.get(complaint_managment.getSelSubCountyofWarehouse().toString())
								: "--";
				complaint_managment.setSelSubCountyofWarehouseVal(dynamicValue.toString());

				dynamicValue = (selWarehouseShopName.has(complaint_managment.getSelWarehouseShopName().toString()))
						? selWarehouseShopName.get(complaint_managment.getSelWarehouseShopName().toString())
						: "--";
				complaint_managment.setSelWarehouseShopNameVal(dynamicValue.toString());

				dynamicValue = (selNameofGrader.has(complaint_managment.getSelNameofGrader().toString()))
						? selNameofGrader.get(complaint_managment.getSelNameofGrader().toString())
						: "--";
				complaint_managment.setSelNameofGraderVal(dynamicValue.toString());
				dynamicValue = (selNameofCollateralManager
						.has(complaint_managment.getSelNameofCollateralManager().toString()))
								? selNameofCollateralManager
										.get(complaint_managment.getSelNameofCollateralManager().toString())
								: "--";
				complaint_managment.setSelNameofCollateralManagerVal(dynamicValue.toString());
				dynamicValue = (selNameofInspector.has(complaint_managment.getSelNameofInspector().toString()))
						? selNameofInspector.get(complaint_managment.getSelNameofInspector().toString())
						: "--";
				complaint_managment.setSelNameofInspectorVal(dynamicValue.toString());
				dynamicValue = (selTypeofComplain.has(complaint_managment.getSelTypeofComplain().toString()))
						? selTypeofComplain.get(complaint_managment.getSelTypeofComplain().toString())
						: "--";
				complaint_managment.setSelTypeofComplainVal(dynamicValue.toString());

				complaint_managmentList.add(complaint_managment);
				jsonOb.put("result1", new JSONArray(complaint_managmentList));
				complaint_managmentList.remove(0);
				jsonArray.put(jsonOb);
			}
		}

		JSONObject resultJson = new JSONObject();
		resultJson.put("status", 200);
		resultJson.put("result", jsonArray);
		resultJson.put("count", obj.getTotalElements());
		return resultJson;

	}

	@Override
	public JSONObject getEventTakeActionDetails(String data) throws Exception {
		logger.info("Inside getEventTakeActionDetails method of ContactformServiceImpl");
		JSONObject getAuthArr = new JSONObject();
		JSONObject requestedData = new JSONObject(data);
		Long currentTime = System.currentTimeMillis();

		Integer intId = requestedData.optInt("intId", 0);
		Integer onlineServiceId = requestedData.optInt("onlineServiceId", 0);
		Integer stageNo = requestedData.optInt("stageNo", 0);
		Integer action = requestedData.optInt("action", 0);
		String remark = requestedData.optString("remark", "");
		Integer roleId = requestedData.optInt("roleId", 0);
		Integer userId = requestedData.optInt("userId", 0);
		Integer forwardAuthority = requestedData.optInt("forwardAuthority", 0);
		Integer labelId = requestedData.getInt("labelId");
		String allActions = requestedData.optString("allActions", "");
		String actionType = requestedData.optString("actionType", "0");
		String conditionActions = requestedData.optString("actionCondition", "nocondition");
		String txtInspectorFilePath = requestedData.optString("txtInspectorFilePath", "");
		Integer inspObservation = requestedData.optInt("inspObservation", 0);
		String dateOfReportGen = requestedData.optString("dateOfReportGen", "");
		String isOicReasonValid = requestedData.optString("isOicReasonValid", "");
		String observation = requestedData.optString("observation", "");
		String conditionsToRemoveRevoc = requestedData.optString("conditionsToRemoveRevoc", "");
		JSONArray observationResponse = requestedData.optJSONArray("observationResponse");
		JSONArray verifyDocs = requestedData.optJSONArray("veriFyDocumentList");
		Integer inspectorId = requestedData.optInt("inspectorId", 0);

		String status = "";
		String outMsg = "";
		Integer prevRoleId = roleId;
		try {
			TOnlineServiceApproval actionDetails = tOnlineServiceApprovalRepository
					.getAllDataByUsingIntIdAndOnlineServiceId(intId, onlineServiceId);
			if (actionDetails.getIntForwardedUserId() > 0) {
				prevRoleId = actionDetails.getIntSentFrom();
			}

			String vchAuths = tSetAuthorityRepository.getVchAuthsByProcessIdAndRoleId(intId, prevRoleId,
					actionDetails.getIntStageNo(), labelId);

			String[] vchAuthList = vchAuths.split(",");
			boolean isPresent = false;
			boolean isGenerate = false;
			for (String num : vchAuthList) {
				int currentNumber = Integer.parseInt(num.trim());
				if (currentNumber == 13) {
					isPresent = true;

				}
				if (currentNumber == 14) {
					isGenerate = true;
				}
			}
			Date date = new Date(currentTime);

			String[] pendingData = actionDetails.getIntPendingAt().split(",");
			List<String> intPending = new ArrayList<>();
			for (String value : pendingData) {
				intPending.add(value);
			}
			if (actionDetails != null) {
				OnlineServiceApprovalNotings notings = new OnlineServiceApprovalNotings();
				notings.setIntProcessId(intId);
				notings.setIntProfileId(0);
				notings.setIntOnlineServiceId(onlineServiceId);
				notings.setIntFromAuthority(roleId);
				notings.setDtActionTaken(date);
				notings.setIntStatus(action);
				notings.setTinStageCtr(stageNo);
				notings.setTxtNoting(remark);
				notings.setIntCreatedBy(userId);
				if (roleId == 3) {
					if (txtInspectorFilePath != null&&txtInspectorFilePath.length()>0) {
						byte[] decodedFile = Base64.getDecoder().decode(txtInspectorFilePath);
						String filePath = DocumentUploadutil.uploadFileByte(
								"INSPECTOR_INSPECTION_REPORT_" + System.currentTimeMillis(), decodedFile,
								FolderAndDirectoryConstant.COMPLAIN_MGMT_INSPECTOR_REPORT_FOLDER);
						ComplaintMgmtInspScheduleDocs doc = new ComplaintMgmtInspScheduleDocs();
						doc.setDocPath(filePath);
						doc.setDocName(filePath.substring(filePath.lastIndexOf('/') + 1));
						doc.setBitDeletedFlag(false);
						ComplaintMgmtInspScheduleDocs savedData = docRepo.save(doc);
						notings.setInspectionDocId(savedData.getDocId());
					} else {
						notings.setInspectionDocId(null);
					}
				}else if(roleId == 4 || roleId == 12 && stageNo == 6 && labelId == 104||labelId == 101) {
					if (txtInspectorFilePath != null&&txtInspectorFilePath.length()>0) {
						byte[] decodedFile = Base64.getDecoder().decode(txtInspectorFilePath);
						String filePath = DocumentUploadutil.uploadFileByte(
								"HEARING_DOC_" + System.currentTimeMillis(), decodedFile,
								FolderAndDirectoryConstant.COMPLAIN_MGMT_INSPECTOR_REPORT_FOLDER);
						ComplaintMgmtInspScheduleDocs doc = new ComplaintMgmtInspScheduleDocs();
						doc.setDocPath(filePath);
						doc.setDocName(filePath.substring(filePath.lastIndexOf('/') + 1));
						doc.setBitDeletedFlag(false);
						ComplaintMgmtInspScheduleDocs savedData = docRepo.save(doc);
						notings.setInspectionDocId(savedData.getDocId());
					} else {
						notings.setInspectionDocId(null);
					}
				}
				else {
					notings.setInspectionDocId(null);
				}
				if (roleId == 3 && labelId == 104 && stageNo == 3) {
					SimpleDateFormat formatter = new SimpleDateFormat("yyyy-MM-dd");
					Date reportGenDate = formatter.parse(dateOfReportGen);
					notings.setDateOfReportGen(reportGenDate);
					notings.setInspObservation(inspObservation);
				} else {
					notings.setDateOfReportGen(null);
					notings.setInspObservation(null);
				}
				if (roleId == 5 && labelId == 104 && stageNo == 5) {
					notings.setIsOicReasonValid(isOicReasonValid);
					notings.setObservation(observation);
					notings.setConditionsToRemoveRevoc(conditionsToRemoveRevoc);
				} else {
					notings.setIsOicReasonValid(null);
					notings.setObservation(null);
					notings.setConditionsToRemoveRevoc(null);
				}

				TOnlineServiceApproval onlineServiceApproval = tOnlineServiceApprovalRepository
						.findByIntProcessIdAndIntOnlineServiceIdAndBitDeletedFlag(intId, onlineServiceId, labelId);

				onlineServiceApproval.setTinStatus(action);
				onlineServiceApproval.setDtmStatusDate(date);
				onlineServiceApproval.setIntCreatedBy(userId);
				if(stageNo == 1) {
					Complaint_managment compManEntity = complaint_managmentRepository
							.findByIntIdAndBitDeletedFlag(onlineServiceId, false);
					compManEntity.setApplicationStatus(ComplaintApplicationStatus.IN_PROGRESS);
					complaint_managmentRepository.save(compManEntity);
				}
				if (action == 1) { // Markup action
					if (intPending.size() > 1) {
						intPending.remove(roleId.toString());
						String intPendingString = String.join(",", intPending);

						notings.setIntToAuthority(intPendingString);

						onlineServiceApproval.setIntStageNo(stageNo);
						onlineServiceApproval.setIntATAProcessId(actionDetails.getIntATAProcessId());
						onlineServiceApproval.setIntSentFrom(roleId);
						onlineServiceApproval.setVchForwardAllAction(null);

						if (isPresent || isGenerate) {
							onlineServiceApproval.setIntPendingAt(onlineServiceApproval.getIntPendingAt());
							onlineServiceApproval.setIntForwardTo(onlineServiceApproval.getIntForwardTo());
						} else {
							onlineServiceApproval.setIntPendingAt(intPendingString);
							onlineServiceApproval.setIntForwardTo(intPendingString);
							onlineServiceApproval.setIntForwardedUserId(0);
						}
					} else {

						if (isPresent || isGenerate) {
							getAuthArr = fnAuthority.getAuthorityByCondition(intId, stageNo, labelId, conditionActions);
							String pendingAt = getAuthArr.getString(PENDING_AUTHORITIES);
							Integer newStageNo = getAuthArr.getInt(STAGENO);
							Integer intATAProcessId = getAuthArr.getInt(ATAPROCESSID);
							notings.setIntToAuthority(pendingAt.toString());

							onlineServiceApproval.setIntStageNo(newStageNo);
							onlineServiceApproval.setIntATAProcessId(intATAProcessId);
							onlineServiceApproval.setIntForwardTo(pendingAt);
							onlineServiceApproval.setTinVerifiedBy(roleId);
							onlineServiceApproval.setVchForwardAllAction(null);

						} else {

							if ((labelId == 101 || labelId == 104) && stageNo == 4) {

								if (actionType.equalsIgnoreCase("Suspension")
										&& conditionActions.equalsIgnoreCase("NO")) {
									complaint_managmentRepository.UpdateActionOnApplication(actionType,
											conditionActions, onlineServiceId);
									getAuthArr = fnAuthority.getAuthorityByCondition(intId, stageNo + 1, labelId,
											conditionActions);
								} else if (actionType.equalsIgnoreCase("Revocation")
										&& conditionActions.equalsIgnoreCase("YES")) {
									complaint_managmentRepository.UpdateActionOnApplication(actionType,
											conditionActions, onlineServiceId);
									getAuthArr = fnAuthority.getAuthorityByCondition(intId, stageNo + 1, labelId,
											conditionActions);

								}
							} else if ((labelId == 101 || labelId == 104) && stageNo == 5) {

								if (actionType.equalsIgnoreCase("Suspension")
										&& conditionActions.equalsIgnoreCase("BOTH")) {
									complaint_managmentRepository.UpdateActionOnApplication(actionType,
											conditionActions, onlineServiceId);
									getAuthArr = fnAuthority.getAuthorityByCondition(intId, stageNo + 1, labelId,
											conditionActions);
								} else if (actionType.equalsIgnoreCase("Revocation")
										&& conditionActions.equalsIgnoreCase("BOTH")) {
									complaint_managmentRepository.UpdateActionOnApplication(actionType,
											conditionActions, onlineServiceId);
									getAuthArr = fnAuthority.getAuthorityByCondition(intId, stageNo + 1, labelId,
											conditionActions);

								} else if (actionType.equalsIgnoreCase("Suspension")
										&& conditionActions.equalsIgnoreCase("NO")) {
									complaint_managmentRepository.UpdateActionOnApplication(actionType,
											conditionActions, onlineServiceId);
									getAuthArr = fnAuthority.getAuthorityByCondition(intId, stageNo + 1, labelId,
											conditionActions);
								}

							} else {

								getAuthArr = fnAuthority.getAuthority(intId, stageNo + 1, labelId);
							}

							if (getAuthArr != null) {
								String pendingAt = getAuthArr.getString(PENDING_AUTHORITIES);
								Integer newStageNo = getAuthArr.getInt(STAGENO);
								Integer intATAProcessId = getAuthArr.getInt(ATAPROCESSID);
								notings.setIntToAuthority(pendingAt.toString());

								onlineServiceApproval.setIntStageNo(newStageNo);
								onlineServiceApproval.setIntPendingAt(pendingAt.toString());
								onlineServiceApproval.setIntATAProcessId(intATAProcessId);
								onlineServiceApproval.setIntForwardTo(pendingAt);
								onlineServiceApproval.setTinVerifiedBy(0);
								onlineServiceApproval.setIntSentFrom(roleId);
								onlineServiceApproval.setVchForwardAllAction(null);
								onlineServiceApproval.setIntForwardedUserId(0);
							}
						}
					}
					if ((labelId == 101 || labelId == 103 || labelId == 104) && stageNo == 2) {
						ComplaintMgmtInspSchedule entity = new ComplaintMgmtInspSchedule();
						entity.setComplaintId(onlineServiceId);
						entity.setInspectorId(inspectorId);
						Tuser user = tuserRepository.findById(inspectorId).get();
						entity.setIntRoleId(user.getSelRole());
						entity.setInspectorName(user.getTxtFullName());
						entity.setVchInspStatus("NotStarted");
						compMgmtRepo.save(entity);
					}
					if (roleId == 3) {
						ComplaintMgmtInspSchedule entity = compMgmtRepo.findByComplaintId(onlineServiceId);
						if (entity.getVchInspStatus().equals("Completed")) {
							entity.setBitTakeActionStatus(true);
							compMgmtRepo.save(entity);
						}
					}
					status = "200";
					outMsg = "Markup Action Taken Successfully!";
				} else if (action == 2) {// MarkDown
					if (intPending.size() > 1) {
						intPending.remove(roleId.toString());
						String intPendingString = String.join(",", intPending);
						notings.setIntToAuthority(intPendingString);

						onlineServiceApproval.setIntStageNo(stageNo);
						onlineServiceApproval.setIntPendingAt(intPendingString);
						onlineServiceApproval.setIntATAProcessId(actionDetails.getIntATAProcessId());
						onlineServiceApproval.setIntForwardTo(intPendingString);
						onlineServiceApproval.setIntSentFrom(roleId);
						onlineServiceApproval.setIntForwardedUserId(0);
					} else {
						getAuthArr = fnAuthority.getAuthority(intId, stageNo - 1, labelId);
						if (getAuthArr != null) {
							String pendingAt = getAuthArr.getString(PENDING_AUTHORITIES);
							Integer newStageNo = getAuthArr.getInt(STAGENO);
							Integer intATAProcessId = getAuthArr.getInt(ATAPROCESSID);
							notings.setIntToAuthority(pendingAt.toString());

							onlineServiceApproval.setIntStageNo(newStageNo);
							onlineServiceApproval.setIntPendingAt(pendingAt.toString());
							onlineServiceApproval.setIntATAProcessId(intATAProcessId);
							onlineServiceApproval.setIntForwardTo(pendingAt);
							onlineServiceApproval.setTinVerifiedBy(roleId);
							onlineServiceApproval.setIntSentFrom(roleId);
							onlineServiceApproval.setIntForwardedUserId(0);
						}
					}
					status = "200";
					outMsg = "MarkDown Action Taken Successfully!";
				} else if (action == 3) {// Resubmit
					notings.setIntToAuthority("0");
					notings.setTinStageCtr(0);
					OnlineServiceApprovalNotings onlineServiceApprovalNotings = onlineServiceApprovalNotingsRepository
							.saveAndFlush(notings);

					onlineServiceApproval.setIntStageNo(0);
					onlineServiceApproval.setIntPendingAt("0");
					onlineServiceApproval.setIntATAProcessId(0);
					onlineServiceApproval.setIntForwardTo("0");
					onlineServiceApproval.setTinResubmitStatus(1);
					onlineServiceApproval.setIntSentFrom(roleId);
					onlineServiceApproval.setVchForwardAllAction(null);
					onlineServiceApproval.setIntForwardedUserId(0);
					onlineServiceApproval.setVchForwardAllAction(null);
					onlineServiceApproval.setIntForwardedUserId(0);
					TOnlineServiceApplicationH tOnlineServiceApplicationH = new TOnlineServiceApplicationH();
					tOnlineServiceApplicationH.setIntOnlineServiceId(onlineServiceId);
					tOnlineServiceApplicationH.setIntNotingId(onlineServiceApprovalNotings.getIntNotingsId());
					tOnlineServiceApplicationH.setIntProcessId(intId);
					tOnlineServiceApplicationH.setIntCreatedBy(userId);
					tOnlineServiceApplicationH.setTinResubmitStatus((short) 1);
					tOnlineServiceApplicationHRepository.save(tOnlineServiceApplicationH);
					status = "200";
					outMsg = "Resubmit Action Taken Successfully!";
				} else if (action == 4) {// Forward
					getAuthArr = fnAuthority.getAuthority(intId, stageNo, labelId);
					Integer forwardedRoleId = tuserRepository.findByRoleIdByUserId(forwardAuthority);
					if (getAuthArr != null) {
						String pendingAt = getAuthArr.getString(PENDING_AUTHORITIES);
						Integer newStageNo = getAuthArr.getInt(STAGENO);
						Integer intATAProcessId = getAuthArr.getInt(ATAPROCESSID);
						notings.setIntFromAuthority(roleId);
						notings.setIntToAuthority(forwardedRoleId.toString());
						notings.setTinQueryTo(forwardAuthority);

						onlineServiceApproval.setIntStageNo(newStageNo);
						onlineServiceApproval.setIntPendingAt(forwardedRoleId.toString());
						onlineServiceApproval.setIntATAProcessId(intATAProcessId);
						onlineServiceApproval.setIntForwardTo(forwardedRoleId.toString());
						onlineServiceApproval.setVchForwardAllAction(allActions);
						onlineServiceApproval.setIntForwardedUserId(forwardAuthority);
						onlineServiceApproval.setTinVerifiedBy(roleId);
						onlineServiceApproval.setIntSentFrom(roleId);

					}

					status = "200";
					outMsg = "Forward Action Taken Successfully!";
				} else if (action == 7) {// Reject
					notings.setIntToAuthority("0");
					notings.setTinStageCtr(0);

					onlineServiceApproval.setIntStageNo(0);
					onlineServiceApproval.setIntPendingAt("0");
					onlineServiceApproval.setIntATAProcessId(0);
					onlineServiceApproval.setIntForwardTo("0");
					onlineServiceApproval.setTinResubmitStatus(0);
					onlineServiceApproval.setIntForwardedUserId(0);
					List<Map<String, Object>> mapObjlist = workflowServiceImpl.getallApprovalAction();

					if (labelId == 101 && roleId == 13 && stageNo == 7) {

						String actionName = mapObjlist.stream()
								.filter(x -> Integer.parseInt(x.get("tinApprovalActionId").toString()) == 17)
								.collect(Collectors.toList()).get(0).get("vchActionName").toString();
						Complaint_managment comp_man_entity = complaint_managmentRepository
								.findByIntIdAndBitDeletedFlag(onlineServiceId, false);
						comp_man_entity.setApplicationStatus(ComplaintApplicationStatus.fromActionName(actionName));
						complaint_managmentRepository.save(comp_man_entity);
					}

					if (labelId == 101 && roleId == 5 && stageNo == 6) {
						String actionName = mapObjlist.stream()
								.filter(x -> Integer.parseInt(x.get("tinApprovalActionId").toString()) == 15)
								.collect(Collectors.toList()).get(0).get("vchActionName").toString();
						Complaint_managment comp_man_entity = complaint_managmentRepository
								.findByIntIdAndBitDeletedFlag(onlineServiceId, false);
						comp_man_entity.setApplicationStatus(ComplaintApplicationStatus.fromActionName(actionName));
						complaint_managmentRepository.save(comp_man_entity);
					}

					if (labelId == 102 && roleId == 5 && stageNo == 4) {

						String actionName = mapObjlist.stream()
								.filter(x -> Integer.parseInt(x.get("tinApprovalActionId").toString()) == 15)
								.collect(Collectors.toList()).get(0).get("vchActionName").toString();
						Complaint_managment comp_man_entity = complaint_managmentRepository
								.findByIntIdAndBitDeletedFlag(onlineServiceId, false);
						comp_man_entity.setApplicationStatus(ComplaintApplicationStatus.fromActionName(actionName));
						complaint_managmentRepository.save(comp_man_entity);
					}

					if (labelId == 103 && roleId == 5 && stageNo == 6) {

						String actionName = mapObjlist.stream()
								.filter(x -> Integer.parseInt(x.get("tinApprovalActionId").toString()) == 15)
								.collect(Collectors.toList()).get(0).get("vchActionName").toString();
						Complaint_managment comp_man_entity = complaint_managmentRepository
								.findByIntIdAndBitDeletedFlag(onlineServiceId, false);
						comp_man_entity.setApplicationStatus(ComplaintApplicationStatus.fromActionName(actionName));
						complaint_managmentRepository.save(comp_man_entity);
					}

					if (labelId == 104 && roleId == 5 && stageNo == 7) {

						String actionName = mapObjlist.stream()
								.filter(x -> Integer.parseInt(x.get("tinApprovalActionId").toString()) == 17)
								.collect(Collectors.toList()).get(0).get("vchActionName").toString();
						Complaint_managment comp_man_entity = complaint_managmentRepository
								.findByIntIdAndBitDeletedFlag(onlineServiceId, false);
						comp_man_entity.setApplicationStatus(ComplaintApplicationStatus.fromActionName(actionName));
						complaint_managmentRepository.save(comp_man_entity);
					}

					status = "200";
					outMsg = "Reject Action Taken Successfully!";
				} else if (action == 8) {// Approve
					if (isPresent || isGenerate) {
						notings.setTinStageCtr(stageNo);

						onlineServiceApproval.setIntStageNo(stageNo);
						onlineServiceApproval.setIntForwardTo("0");
					} else {
						notings.setIntToAuthority("0");
						notings.setTinStageCtr(0);
						
						onlineServiceApproval.setIntStageNo(0);
						onlineServiceApproval.setIntPendingAt("0");
						onlineServiceApproval.setIntATAProcessId(0);
						onlineServiceApproval.setIntForwardTo("0");
						onlineServiceApproval.setTinResubmitStatus(0);
						onlineServiceApproval.setIntForwardedUserId(0);

					}

					List<Map<String, Object>> mapObjlist = workflowServiceImpl.getallApprovalAction();
					if (labelId == 101 && roleId == 13 && stageNo == 7) {

						String actionName = mapObjlist.stream()
								.filter(x -> Integer.parseInt(x.get("tinApprovalActionId").toString()) == 16)
								.collect(Collectors.toList()).get(0).get("vchActionName").toString();
						Complaint_managment comp_man_entity = complaint_managmentRepository
								.findByIntIdAndBitDeletedFlag(onlineServiceId, false);
						comp_man_entity.setApplicationStatus(ComplaintApplicationStatus.fromActionName(actionName));
						complaint_managmentRepository.save(comp_man_entity);
//						Optional<Tuser> userOpt = tuserRepository.findById(Integer.parseInt(comp_man_entity.getSelNam()));
//						if(userOpt.isPresent()) {
//							Tuser user = userOpt.get();
//							user.setVchUserStatus(actionName);
//							tuserRepository.save(user);
//						}
					}

					if (labelId == 101 && roleId == 5 && stageNo == 6) {
						String actionName = mapObjlist.stream()
								.filter(x -> Integer.parseInt(x.get("tinApprovalActionId").toString()) == 8)
								.collect(Collectors.toList()).get(0).get("vchActionName").toString();
						Complaint_managment comp_man_entity = complaint_managmentRepository
								.findByIntIdAndBitDeletedFlag(onlineServiceId, false);
						comp_man_entity.setApplicationStatus(ComplaintApplicationStatus.fromActionName(actionName));
						complaint_managmentRepository.save(comp_man_entity);
//						Optional<Tuser> userOpt = tuserRepository.findById(Integer.parseInt(comp_man_entity.getSelNameofInspector()));
//						if(userOpt.isPresent()) {
//							Tuser user = userOpt.get();
//							user.setVchUserStatus(actionName);
//							tuserRepository.save(user);
//						}
					}

					if (labelId == 102 && roleId == 5 && stageNo == 4) {

						String actionName = mapObjlist.stream()
								.filter(x -> Integer.parseInt(x.get("tinApprovalActionId").toString()) == 8)
								.collect(Collectors.toList()).get(0).get("vchActionName").toString();
						Complaint_managment comp_man_entity = complaint_managmentRepository
								.findByIntIdAndBitDeletedFlag(onlineServiceId, false);
						comp_man_entity.setApplicationStatus(ComplaintApplicationStatus.fromActionName(actionName));
						complaint_managmentRepository.save(comp_man_entity);
						Optional<Tuser> userOpt = tuserRepository.findById(Integer.parseInt(comp_man_entity.getSelNameofInspector()));
						if(userOpt.isPresent()) {
							Tuser user = userOpt.get();
							user.setVchUserStatus(actionName);
							tuserRepository.save(user);
						}
					}

					if (labelId == 103 && roleId == 5 && stageNo == 6) {

						String actionName = mapObjlist.stream()
								.filter(x -> Integer.parseInt(x.get("tinApprovalActionId").toString()) == 8)
								.collect(Collectors.toList()).get(0).get("vchActionName").toString();
						Complaint_managment comp_man_entity = complaint_managmentRepository
								.findByIntIdAndBitDeletedFlag(onlineServiceId, false);
						comp_man_entity.setApplicationStatus(ComplaintApplicationStatus.fromActionName(actionName));
						complaint_managmentRepository.save(comp_man_entity);
						Optional<Tuser> userOpt = tuserRepository.findById(Integer.parseInt(comp_man_entity.getSelNameofGrader()));
						if(userOpt.isPresent()) {
							Tuser user = userOpt.get();
							user.setVchUserStatus(actionName);
							tuserRepository.save(user);
						}
					}

					if (labelId == 104 && roleId == 5 && stageNo == 7) {

						String actionName = mapObjlist.stream()
								.filter(x -> Integer.parseInt(x.get("tinApprovalActionId").toString()) == 16)
								.collect(Collectors.toList()).get(0).get("vchActionName").toString();
						Complaint_managment comp_man_entity = complaint_managmentRepository.findByIntIdAndBitDeletedFlag(onlineServiceId, false);
						comp_man_entity.setApplicationStatus(ComplaintApplicationStatus.fromActionName(actionName));
						complaint_managmentRepository.save(comp_man_entity);
						Optional<Tuser> userOpt = tuserRepository.findById(Integer.parseInt(comp_man_entity.getSelNameofCollateralManager()));
						if(userOpt.isPresent()) {
							Tuser user = userOpt.get();
							user.setVchUserStatus(actionName);
							tuserRepository.save(user);
						}
						
						TapplicationOfCertificateOfCompliance collateral_details =repo.findByIntAdminUserIdAndBitDeletedFlag(Integer.parseInt(comp_man_entity.getSelNameofCollateralManager()), false);
						
						
						collateral_details.setCollateralManagerStatus(CollateralManagerStatus.Revoke);
						 Integer timesuspend=collateral_details.getTimesSuspended();
						 collateral_details.setTimesSuspended(timesuspend+1);						
						
						repo.save(collateral_details);
						
						
					}
					
					
					if (labelId == 104 && roleId == 5 && stageNo == 6) {

						String actionName = mapObjlist.stream()
								.filter(x -> Integer.parseInt(x.get("tinApprovalActionId").toString()) == 8)
								.collect(Collectors.toList()).get(0).get("vchActionName").toString();
						Complaint_managment comp_man_entity = complaint_managmentRepository
								.findByIntIdAndBitDeletedFlag(onlineServiceId, false);
						comp_man_entity.setApplicationStatus(ComplaintApplicationStatus.fromActionName(actionName));
						complaint_managmentRepository.save(comp_man_entity);
						Optional<Tuser> userOpt = tuserRepository.findById(Integer.parseInt(comp_man_entity.getSelNameofCollateralManager()));
						if(userOpt.isPresent()) {
							Tuser user = userOpt.get();
							user.setVchUserStatus(actionName);
							tuserRepository.save(user);
						}
						
                         TapplicationOfCertificateOfCompliance collateral_details =repo.findByIntAdminUserIdAndBitDeletedFlag(Integer.parseInt(comp_man_entity.getSelNameofCollateralManager()), false);
						
						collateral_details.setCollateralManagerStatus(CollateralManagerStatus.Suspend);
						 Integer timesuspend=collateral_details.getTimesSuspended();
						 collateral_details.setTimesSuspended(timesuspend+1);						
						repo.save(collateral_details);
						
					}
					

					status = "200";
					outMsg = "Approve Action Taken Successfully!";
				} else if (action == 6) {// Query
					Integer intATAProcessId = actionDetails.getIntATAProcessId();
					Integer selAuthority = 0;
					if (intATAProcessId == 1) {

					} else {
						selAuthority = 0;
					}
					notings.setIntToAuthority(selAuthority.toString());
					notings.setTinStageCtr(0);
					notings.setTinStageCtr(actionDetails.getIntStageNo());

					onlineServiceApproval.setIntStageNo(0);
					onlineServiceApproval.setVchATAAuths(onlineServiceApproval.getIntPendingAt());
					onlineServiceApproval.setIntPendingAt(selAuthority.toString());
					onlineServiceApproval.setIntATAProcessId(0);
					onlineServiceApproval.setIntForwardTo("0");
					onlineServiceApproval.setTinQueryTo(intATAProcessId);
					onlineServiceApproval.setTinResubmitStatus(0);

					status = "200";
					outMsg = "Query Action Taken Successfully!";
				} else if (action == 11) {// Verify action

					if (intPending.size() > 1) {
						intPending.remove(roleId.toString());
						String intPendingString = String.join(",", intPending);

						notings.setIntToAuthority(intPendingString);
						onlineServiceApproval.setIntStageNo(stageNo);
						onlineServiceApproval.setVchForwardAllAction(null);
						if (isPresent || isGenerate) {
							onlineServiceApproval.setIntPendingAt(onlineServiceApproval.getIntPendingAt());
							onlineServiceApproval.setIntForwardTo(onlineServiceApproval.getIntForwardTo());
							onlineServiceApproval.setTinVerifiedBy(roleId);
						} else {
							onlineServiceApproval.setIntPendingAt(intPendingString);
							onlineServiceApproval.setIntForwardTo(intPendingString);
							onlineServiceApproval.setTinVerifiedBy(roleId);
							onlineServiceApproval.setIntForwardedUserId(0);
						}
						onlineServiceApproval.setIntATAProcessId(actionDetails.getIntATAProcessId());
						// onlineServiceApproval.setTinVerifiedBy(roleId);
						onlineServiceApproval.setIntSentFrom(roleId);
					} else {

						if (isPresent || isGenerate) {
							getAuthArr = fnAuthority.getAuthority(intId, stageNo, labelId);
							String pendingAt = getAuthArr.getString(PENDING_AUTHORITIES);
							Integer newStageNo = getAuthArr.getInt(STAGENO);
							Integer intATAProcessId = getAuthArr.getInt(ATAPROCESSID);
							notings.setIntToAuthority(pendingAt.toString());

							onlineServiceApproval.setIntStageNo(newStageNo);
							onlineServiceApproval.setIntATAProcessId(intATAProcessId);
							onlineServiceApproval.setVchForwardAllAction(null);
							onlineServiceApproval.setTinVerifiedBy(roleId);

						} else {

							if ((labelId == 101 || labelId == 104) && stageNo == 4) {

								if (actionType.equalsIgnoreCase("Suspension")
										&& conditionActions.equalsIgnoreCase("NO")) {
									complaint_managmentRepository.UpdateActionOnApplication(actionType,
											conditionActions, onlineServiceId);
									getAuthArr = fnAuthority.getAuthorityByCondition(intId, stageNo + 1, labelId,
											conditionActions);
								} else if (actionType.equalsIgnoreCase("Revocation")
										&& conditionActions.equalsIgnoreCase("YES")) {
									complaint_managmentRepository.UpdateActionOnApplication(actionType,
											conditionActions, onlineServiceId);
									getAuthArr = fnAuthority.getAuthorityByCondition(intId, stageNo + 1, labelId,conditionActions);

								}
							} else if ((labelId == 101 || labelId == 104) && stageNo == 5) {

								if (actionType.equalsIgnoreCase("Suspension")
										&& conditionActions.equalsIgnoreCase("BOTH")) {
									complaint_managmentRepository.UpdateActionOnApplication(actionType,
											conditionActions, onlineServiceId);
									getAuthArr = fnAuthority.getAuthorityByCondition(intId, stageNo + 1, labelId,
											conditionActions);
								} else if (actionType.equalsIgnoreCase("Revocation")
										&& conditionActions.equalsIgnoreCase("BOTH")) {
									complaint_managmentRepository.UpdateActionOnApplication(actionType,
											conditionActions, onlineServiceId);
									getAuthArr = fnAuthority.getAuthorityByCondition(intId, stageNo + 1, labelId,
											conditionActions);

								} else if (actionType.equalsIgnoreCase("Suspension")
										&& conditionActions.equalsIgnoreCase("NO")) {
									complaint_managmentRepository.UpdateActionOnApplication(actionType,
											conditionActions, onlineServiceId);
									getAuthArr = fnAuthority.getAuthorityByCondition(intId, stageNo + 1, labelId,
											conditionActions);
								}

							} else {

								getAuthArr = fnAuthority.getAuthority(intId, stageNo + 1, labelId);
							}

							if (getAuthArr != null) {
								String pendingAt = getAuthArr.getString(PENDING_AUTHORITIES);
								Integer newStageNo = getAuthArr.getInt(STAGENO);
								Integer intATAProcessId = getAuthArr.getInt(ATAPROCESSID);

								notings.setIntToAuthority(pendingAt);

								onlineServiceApproval.setIntStageNo(newStageNo);

								onlineServiceApproval.setIntPendingAt(pendingAt);

								onlineServiceApproval.setIntATAProcessId(intATAProcessId);

								onlineServiceApproval.setIntSentFrom(roleId);
								onlineServiceApproval.setVchForwardAllAction(null);
								onlineServiceApproval.setTinVerifiedBy(0);
								onlineServiceApproval.setIntForwardedUserId(0);
							}

						}

					}

					if ((labelId == 101 || labelId == 103 || labelId == 104) && stageNo == 2) {
						ComplaintMgmtInspSchedule entity = new ComplaintMgmtInspSchedule();
						entity.setComplaintId(onlineServiceId);
						entity.setInspectorId(inspectorId);
						Tuser user = tuserRepository.findById(inspectorId).get();
						entity.setIntRoleId(user.getSelRole());
						entity.setInspectorName(user.getTxtFullName());
						entity.setVchInspStatus("NotStarted");
						compMgmtRepo.save(entity);
					}

					status = "200";
					outMsg = "Verify Action Taken Successfully!";

				}
				if (action != 3) {
					OnlineServiceApprovalNotings onlineServiceApprovalNotings = onlineServiceApprovalNotingsRepository
							.saveAndFlush(notings);
					if (observationResponse != null) {
						for (int i = 0; i < observationResponse.length(); i++) {
							JSONObject jsonObject = observationResponse.getJSONObject(i);
							ComplaintObservationResponse compResponse = new ComplaintObservationResponse();
							compResponse.setNotingId(onlineServiceApprovalNotings.getIntNotingsId());
							compResponse.setObResRole(jsonObject.getInt("roleId"));
							compResponse.setObResStage(jsonObject.getInt("stageNo"));
							compResponse.setObResAPId(jsonObject.getInt("applicationId"));
							compResponse.setObResLabelId(jsonObject.getString("labelId"));
							compResponse.setObResQuId(jsonObject.getString("questionId"));
							compResponse.setObRes(jsonObject.getString("answer"));
							resRepo.save(compResponse);
						}
					}

				}
				tOnlineServiceApprovalRepository.save(onlineServiceApproval);
			}
		} catch (Exception e) {
			e.printStackTrace();
			status = "500";
			outMsg = "Error occurred during action!";
		}

		JSONObject jsObj = new JSONObject();
		jsObj.put("status", status);
		jsObj.put("outMsg", outMsg);
		jsObj.put("id", intId);
		jsObj.put("result", "");
		return jsObj;

	}

	public JSONObject getPreviewDetails(Integer intId) throws Exception {
		JSONArray arrDetails = new JSONArray();
		String status = "";
		String outMsg = "";

		try {
			Complaint_managment complaint_managment = complaint_managmentRepository.findByIntIdAndBitDeletedFlag(intId,
					false);

			if (complaint_managment != null) {
				dynamicValue = (selCounty.has(complaint_managment.getSelCounty().toString()))
						? selCounty.get(complaint_managment.getSelCounty().toString())
						: "--";
				County county = countyRepo.findById(complaint_managment.getSelCounty()).get();
				complaint_managment.setSelCountyVal(county.getName());
				dynamicValue = (selSubCounty.has(complaint_managment.getSelSubCounty().toString()))
						? selSubCounty.get(complaint_managment.getSelSubCounty().toString())
						: "--";
				SubCounty subCounty = subCountyRepo.findById(complaint_managment.getSelSubCounty()).get();
				complaint_managment.setSelSubCountyVal(subCounty.getTxtSubCountyName());
				dynamicValue = (rdoComplaintAgainst.has(complaint_managment.getRdoComplaintAgainst().toString()))
						? rdoComplaintAgainst.get(complaint_managment.getRdoComplaintAgainst().toString())
						: "--";
				complaint_managment.setRdoComplaintAgainstVal(dynamicValue.toString());
				dynamicValue = (selCountyofWarehouse.has(complaint_managment.getSelCountyofWarehouse().toString()))
						? selCountyofWarehouse.get(complaint_managment.getSelCountyofWarehouse().toString())
						: "--";

				if (!complaint_managment.getSelCountyofWarehouse().toString().equals("0")) {
					County selcounty = countyRepo.findById(complaint_managment.getSelCountyofWarehouse()).get();
					complaint_managment.setSelCountyofWarehouseVal(selcounty.getName());
				}

				dynamicValue = (selSubCountyofWarehouse
						.has(complaint_managment.getSelSubCountyofWarehouse().toString()))
								? selSubCountyofWarehouse
										.get(complaint_managment.getSelSubCountyofWarehouse().toString())
								: "--";
				if (!complaint_managment.getSelSubCountyofWarehouse().toString().equals("0")) {
					SubCounty selsubCounty = subCountyRepo.findById(complaint_managment.getSelSubCountyofWarehouse())
							.get();
					complaint_managment.setSelSubCountyofWarehouseVal(selsubCounty.getTxtSubCountyName());
				}

				dynamicValue = (selWarehouseShopName.has(complaint_managment.getSelWarehouseShopName().toString()))
						? selWarehouseShopName.get(complaint_managment.getSelWarehouseShopName().toString())
						: "--";
				if (!complaint_managment.getSelWarehouseShopName().toString().equals("0")) {
					ApplicationOfConformity app = appRepo.findById(complaint_managment.getSelWarehouseShopName()).get();
					complaint_managment.setSelWarehouseShopNameVal(app.getParticularOfApplicantsId().getShop());
				}

				dynamicValue = (selNameofGrader.has(complaint_managment.getSelNameofGrader().toString()))
						? selNameofGrader.get(complaint_managment.getSelNameofGrader().toString())
						: "--";
				if (!complaint_managment.getSelNameofGrader().toString().equals("0")) {
					Tuser cm = tuserRepository.findById(Integer.parseInt(complaint_managment.getSelNameofGrader()))
							.get();
					complaint_managment.setSelNameofGraderVal(cm.getTxtFullName());
				} else {
					complaint_managment.setSelNameofGraderVal(dynamicValue.toString());
				}
				dynamicValue = (selNameofCollateralManager
						.has(complaint_managment.getSelNameofCollateralManager().toString()))
								? selNameofCollateralManager
										.get(complaint_managment.getSelNameofCollateralManager().toString())
								: "--";
				if (!complaint_managment.getSelNameofCollateralManager().toString().equals("0")) {
					Tuser cm = tuserRepository
							.findById(Integer.parseInt(complaint_managment.getSelNameofCollateralManager())).get();
					complaint_managment.setSelNameofCollateralManagerVal(cm.getTxtFullName());
				} else {
					complaint_managment.setSelNameofCollateralManagerVal(dynamicValue.toString());
				}
				dynamicValue = (selNameofInspector.has(complaint_managment.getSelNameofInspector().toString()))
						? selNameofInspector.get(complaint_managment.getSelNameofInspector().toString())
						: "--";
				if (!complaint_managment.getSelNameofInspector().toString().equals("0")) {
					Tuser insp = tuserRepository.findById(Integer.parseInt(complaint_managment.getSelNameofInspector()))
							.get();
					complaint_managment.setSelNameofInspectorVal(insp.getTxtFullName());
				} else {
					complaint_managment.setSelNameofInspectorVal(dynamicValue.toString());
				}
				dynamicValue = (selTypeofComplain.has(complaint_managment.getSelTypeofComplain().toString()))
						? selTypeofComplain.get(complaint_managment.getSelTypeofComplain().toString())
						: "--";
				ComplaintType ct = ctRepo.findById(Integer.parseInt(complaint_managment.getSelTypeofComplain())).get();
				complaint_managment.setSelTypeofComplainVal(ct.getComplaintType());

// arrDetails.put(new JSONObject().put("label", "Complainants Information").put("value", complaint_managment.getHedComplainantsInformation()));

				arrDetails.put(
						new JSONObject().put("label", "Full Name").put("value", complaint_managment.getTxtFullName()));

				arrDetails.put(new JSONObject().put("label", "Contact Number").put("value",
						complaint_managment.getTxtContactNumber()));

				arrDetails.put(new JSONObject().put("label", "Email Address").put("value",
						complaint_managment.getTxtEmailAddress()));

				arrDetails.put(
						new JSONObject().put("label", "Address").put("value", complaint_managment.getTxtrAddress()));

				arrDetails.put(
						new JSONObject().put("label", "County").put("value", complaint_managment.getSelCountyVal()));

				arrDetails.put(new JSONObject().put("label", "Sub County").put("value",
						complaint_managment.getSelSubCountyVal()));

// arrDetails.put(new JSONObject().put("label", "Complaint Information").put("value", complaint_managment.getHedComplaintInformation()));

				arrDetails.put(new JSONObject().put("label", "Complaint Against").put("value",
						complaint_managment.getRdoComplaintAgainstVal()));

				if (complaint_managment.getSelCountyofWarehouseVal() != null
						&& !("--".equals(complaint_managment.getSelCountyofWarehouseVal()))
						&& !("".equals(complaint_managment.getSelCountyofWarehouseVal()))
						&& !("0".equals(complaint_managment.getSelCountyofWarehouseVal()))) {
					arrDetails.put(new JSONObject().put("label", "County of Warehouse").put("value",
							complaint_managment.getSelCountyofWarehouseVal()));
				}

				if (complaint_managment.getSelSubCountyofWarehouseVal() != null
						&& !("--".equals(complaint_managment.getSelSubCountyofWarehouseVal()))
						&& !("".equals(complaint_managment.getSelSubCountyofWarehouseVal()))
						&& !("0".equals(complaint_managment.getSelSubCountyofWarehouseVal()))) {
					arrDetails.put(new JSONObject().put("label", "Sub-County of Warehouse").put("value",
							complaint_managment.getSelSubCountyofWarehouseVal()));
				}

				if (complaint_managment.getSelWarehouseShopNameVal() != null
						&& !("--".equals(complaint_managment.getSelWarehouseShopNameVal()))
						&& !("".equals(complaint_managment.getSelWarehouseShopNameVal()))
						&& !("0".equals(complaint_managment.getSelWarehouseShopNameVal()))) {
					arrDetails.put(new JSONObject().put("label", "Warehouse Shop Name").put("value",
							complaint_managment.getSelWarehouseShopNameVal()));
				}

				if (complaint_managment.getTxtWarehouseOperator() != null
						&& !("--".equals(complaint_managment.getTxtWarehouseOperator()))
						&& !("".equals(complaint_managment.getTxtWarehouseOperator()))
						&& !("0".equals(complaint_managment.getTxtWarehouseOperator()))) {
					arrDetails.put(new JSONObject().put("label", "Warehouse Operator").put("value",
							complaint_managment.getTxtWarehouseOperator()));
				}

				if (complaint_managment.getSelNameofGraderVal() != null
						&& !("--".equals(complaint_managment.getSelNameofGraderVal()))
						&& !("".equals(complaint_managment.getSelNameofGraderVal()))
						&& !("0".equals(complaint_managment.getSelNameofGraderVal()))) {
					arrDetails.put(new JSONObject().put("label", " Name of Grader").put("value",
							complaint_managment.getSelNameofGraderVal()));
				}

				if (complaint_managment.getSelNameofCollateralManagerVal() != null
						&& !("--".equals(complaint_managment.getSelNameofCollateralManagerVal()))
						&& !("".equals(complaint_managment.getSelNameofCollateralManagerVal()))
						&& !("0".equals(complaint_managment.getSelNameofCollateralManagerVal()))) {
					arrDetails.put(new JSONObject().put("label", "Name of Collateral Manager").put("value",
							complaint_managment.getSelNameofCollateralManagerVal()));
				}

				if (complaint_managment.getSelNameofInspectorVal() != null
						&& !("--".equals(complaint_managment.getSelNameofInspectorVal()))
						&& !("".equals(complaint_managment.getSelNameofInspectorVal()))
						&& !("0".equals(complaint_managment.getSelNameofInspectorVal()))) {
					arrDetails.put(new JSONObject().put("label", "Name of Inspector").put("value",
							complaint_managment.getSelNameofInspectorVal()));
				}

				if (complaint_managment.getSelTypeofComplainVal() != null
						&& !("--".equals(complaint_managment.getSelTypeofComplainVal()))
						&& !("".equals(complaint_managment.getSelTypeofComplainVal()))
						&& !("0".equals(complaint_managment.getSelTypeofComplainVal()))) {
					arrDetails.put(new JSONObject().put("label", "Type of Complain").put("value",
							complaint_managment.getSelTypeofComplainVal()));
				}

				arrDetails.put(new JSONObject().put("label", "Date of Incident ").put("value", complaint_managment
						.getTxtDateofIncident().toLocalDate().format(DateTimeFormatter.ofPattern("dd-MMM-yyyy"))));

				arrDetails.put(new JSONObject().put("label", "Description of Incident").put("value",
						complaint_managment.getTxtrDescriptionofIncident()));

// arrDetails.put(new JSONObject().put("label", "Supporting Documents").put("value", complaint_managment.getHedSupportingDocuments()));

//				arrDetails.put(new JSONObject().put("label",
//								"I hereby declare that the information provided is true and accurate to the best of my knowledge and belief.")
//						.put("value", complaint_managment
//								.getChkdeclartion()));
				status = "200";
				outMsg = "Success!!!";
			}

		} catch (Exception e) {
			e.printStackTrace();
			status = "400";
			outMsg = "Something went Wrong!!!";
		}
		JSONObject result = new JSONObject();
		result.put("status", status);
		result.put("msg", outMsg);
		result.put("result", arrDetails);
		return result;
	}

	@Override
	public JSONObject deleteById(Integer id) throws Exception {
		logger.info("Inside deleteById method of Complaint_managmentServiceImpl");
		Complaint_managment entity = complaint_managmentRepository.findByIntIdAndBitDeletedFlag(id, false);

		entity.setBitDeletedFlag(true);
		complaint_managmentRepository.save(entity);
		List<Supporting_document> supporting_documentList = supporting_documentRepository
				.findByIntParentIdAndBitDeletedFlag(id, false);
		supporting_documentList.forEach(t -> t.setBitDeletedFlag(true));
		supporting_documentRepository.saveAll(supporting_documentList);
		json.put("status", 200);
		return json;
	}

	@Override
	public Page<ComplaintmanagementResponse> getFilteredComplaint(String search, String sortColumn,
			String sortDirection, Pageable sortedPageable) {
		logger.info("Inside getFilteredComplaint method of Complaint_managementServiceImpl");

		Integer totalDataPresent = complaint_managmentRepository.countByBitDeletedFlag(false);
		logger.info("totalDataPresent" + totalDataPresent);

		List<ComplaintApplicationStatus> applicationStatuses =Arrays.asList(ComplaintApplicationStatus.SUSPEND, ComplaintApplicationStatus.REVOKE_LICENSE);
		Page<Complaint_managment> response = complaint_managmentRepository.findByFilters(search, sortedPageable,applicationStatuses);
		logger.info("page contents" + response.getContent());
		logger.info("Total pages" + response.getTotalPages());
		logger.info("Total elements" + response.getTotalElements());

		List<ComplaintmanagementResponse> complaintManagementResponse = response.getContent().stream().map(entity -> {
			ComplaintmanagementResponse dto = Mapper.mapToComplaintmanagementResponse(entity);
			String AOCno = entity.getSelWarehouseShopName();
			String commodityTypes = applicationOfConformityRepository.getCommodityTypes(AOCno, Status.Accepted, false);

			if (commodityTypes == null || commodityTypes.isEmpty()) {
				logger.warn("No commodity types found for AOCno: " + AOCno);
				dto.setTypeOfCommodities("---");
				dto.setOtherCommodities(Collections.emptyList());
			} else {
			logger.info("commodity types" + commodityTypes);
			ObjectMapper objectMapper = new ObjectMapper();
			try {
				List<Commodity> commodities = objectMapper.readValue(commodityTypes, new TypeReference<List<Commodity>>() {
				});
				logger.info("list of commodity" + commodities);
				if (!commodities.isEmpty()) {
					dto.setTypeOfCommodities(commodities.get(0).getName());
					logger.info("commodity inside commodity" + dto.getTypeOfCommodities());
					List<String> otherCommodities = new ArrayList<>();
					for (int i = 1; i < commodities.size(); i++) {
						otherCommodities.add(commodities.get(i).getName());
					}
					dto.setOtherCommodities(otherCommodities);
					logger.info("otherCommodities" + dto.getOtherCommodities());
				}
			} catch (JsonProcessingException e) {
				throw new RuntimeException(e);
			}
		}

			if (entity.getSelWarehouseShopName() != null && !entity.getSelWarehouseShopName().toString().equals("0")) {
				dto.setSelWarehouseShopName(entity.getSelWarehouseShopName());
			} else {
				dto.setSelWarehouseShopName("---");
			}

			if (entity.getTxtWarehouseOperator() != null && !entity.getTxtWarehouseOperator().toString().isEmpty()) {
				dto.setTxtWarehouseOperator(entity.getTxtWarehouseOperator());
			} else {
				dto.setTxtWarehouseOperator("---");
			}

					Integer count = onlineServiceApprovalNotingsRepository.getCountedData(12, entity.getIntId());
					dto.setNotingCount(count);

					TOnlineServiceApproval approval = tOnlineServiceApprovalRepository.findByIntOnlineServiceIdAndIntStageNoAndBitDeletedFlag(160, entity.getIntId(), false);
					if (approval != null) {
						dto.setTinStatus(approval.getTinStatus());
						dto.setIntProcessId(approval.getIntProcessId());
						dto.setResubmitCount(onlineServiceApprovalNotingsRepository.countByIntOnlineServiceIdAndIntProcessIdAndIntStatusAndBitDeletedFlag(entity.getIntId(), approval.getIntProcessId(), 3, false));
						String pendingAtUser = tuserRepository.getPendignAtUser(Integer.parseInt(approval.getIntPendingAt()));
						dto.setPendingATUser(pendingAtUser);
					}
					dto.setSelCountyVal(selCounty.has(entity.getSelCounty().toString()) ? (String) selCounty.get(entity.getSelCounty().toString()) : "--");
					dto.setSelSubCountyVal(selSubCounty.has(entity.getSelSubCounty().toString()) ? (String) selSubCounty.get(entity.getSelSubCounty().toString()) : "--");
					return dto;
				})
				.collect(Collectors.toList());
		return new PageImpl<>(complaintManagementResponse, sortedPageable, response.getTotalElements());
	}

	@Override
	public ComplaintDetailsComprehensive findById(int id) {
		Complaint_managment entity = complaint_managmentRepository.findByIntIdAndBitDeletedFlag(id, false);
		if (entity == null) {
			throw new CustomGeneralException(errorMessages.getFailedToFetch());
		}

		ComplaintmanagementResponse dto = Mapper.mapToComplaintmanagementResponse(entity);

		String AOCno = entity.getSelWarehouseShopName();
		String commodityTypes = applicationOfConformityRepository.getCommodityTypes(AOCno, Status.Accepted, false);

		if (commodityTypes == null || commodityTypes.isEmpty()) {
			logger.warn("No commodity types found for AOCno: " + AOCno);
			dto.setTypeOfCommodities("---");
			dto.setOtherCommodities(Collections.emptyList());
		} else {
			logger.info("commodity types" + commodityTypes);
			ObjectMapper objectMapper = new ObjectMapper();
			try {
				List<Commodity> commodities = objectMapper.readValue(commodityTypes, new TypeReference<List<Commodity>>() {
				});
				logger.info("list of commodity" + commodities);
				if (!commodities.isEmpty()) {
					dto.setTypeOfCommodities(commodities.get(0).getName());
					logger.info("commodity inside commodity" + dto.getTypeOfCommodities());
					List<String> otherCommodities = new ArrayList<>();
					for (int i = 1; i < commodities.size(); i++) {
						otherCommodities.add(commodities.get(i).getName());
					}
					dto.setOtherCommodities(otherCommodities);
					logger.info("otherCommodities" + dto.getOtherCommodities());
				}
			} catch (JsonProcessingException e) {
				throw new RuntimeException(e);
			}
		}

		if (entity.getSelWarehouseShopName() != null && !entity.getSelWarehouseShopName().toString().equals("0")) {
			dto.setSelWarehouseShopName(entity.getSelWarehouseShopName());
		} else {
			dto.setSelWarehouseShopName("---");
		}

		if (entity.getTxtWarehouseOperator() != null && !entity.getTxtWarehouseOperator().toString().isEmpty()) {
			dto.setTxtWarehouseOperator(entity.getTxtWarehouseOperator());
		} else {
			dto.setTxtWarehouseOperator("---");
		}

		List<ComplaintObservationResponse> observationResponse =complaintObservationResponseRepository.findByObResAPIdAndObResStageAndDeleteFlag(id,7,false);
		ComplaintObservationResponse observation = observationResponse.get(0);
		OnlineServiceApprovalNotings approvalNotings = onlineServiceApprovalNotingsRepository.findByIntNotingsIdAndBitDeletedFlag(observation.getNotingId(),false);


		return ComplaintDetailsComprehensive.builder()
				.complaintManagementResponse(dto)
				.approvalNotings(approvalNotings)
				.observationResponses(observationResponse)
				.build();
	}


	@Override
	public List<ComplaintObservation> getComplaintOservation(Integer lableId, Integer RoleId) {

		return observationRepo.findByIntLableIdAndIntRoleId(lableId, RoleId);
	}
}