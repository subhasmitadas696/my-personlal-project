package app.ewarehouse.serviceImpl;

import app.ewarehouse.dto.Mail;
import app.ewarehouse.dto.RoutineComplianceDTO;
import app.ewarehouse.dto.TakeActionRequest;
import app.ewarehouse.entity.*;
import app.ewarehouse.repository.FocusAreaItemRepository;
import app.ewarehouse.util.*;
import com.fasterxml.jackson.core.type.TypeReference;
import jakarta.annotation.PostConstruct;
import org.springframework.data.domain.*;
import org.springframework.transaction.annotation.Transactional;
import app.ewarehouse.exception.CustomGeneralException;
import app.ewarehouse.repository.RoutineComplianceRepository;
import app.ewarehouse.service.RoutineComplianceService;
import com.fasterxml.jackson.databind.ObjectMapper;
import jakarta.validation.ConstraintViolation;
import jakarta.validation.Validator;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.*;
import java.util.stream.Collectors;

@Service
public class RoutineComplianceServiceImpl implements RoutineComplianceService {

    @Autowired
    private RoutineComplianceRepository routineComplianceRepository;
    @Autowired
    private FocusAreaItemRepository focusAreaItemRepository;
    @Autowired
    private UserIdConstants userIdConstants;
    @Autowired
    private ErrorMessages errorMessages;
    @Autowired
    private Validator validator;
    private static final Logger logger = LoggerFactory.getLogger(RoutineComplianceServiceImpl.class);

    @Transactional
    @Override
    public String save(String data, int loggedInUserId) {
        logger.info("Inside save method of RoutineComplianceServiceImpl");

        String decodedData = CommonUtil.inputStreamDecoder(data);

        try {
            RoutineCompliance routineCompliance = new ObjectMapper().readValue(decodedData, RoutineCompliance.class);

            InspectionObjective inspectionObjective = routineCompliance.getInspectionObjective();
            List<FocusAreaItem> focusAreaItems = inspectionObjective.getFocusAreaItems();
            inspectionObjective.setFocusAreaItems(null);
            routineCompliance.setInspectionObjective(inspectionObjective);
            routineCompliance.setIntCurrentRole(userIdConstants.getCeo());
            routineCompliance.setIntCurrentStage(1);
            routineCompliance.setVchInspectionPlan("Draft");

            Set<ConstraintViolation<RoutineCompliance>> violations = validator.validate(routineCompliance);
            if (!violations.isEmpty()) {
                throw new CustomGeneralException(violations);
            }

            RoutineCompliance savedRoutineCompliance = routineComplianceRepository.save(routineCompliance);

            focusAreaItems = focusAreaItems.stream().peek((FocusAreaItem item) -> item.setInspectionObjective(savedRoutineCompliance.getInspectionObjective())).collect(Collectors.toList());

            focusAreaItemRepository.saveAll(focusAreaItems);

            return routineCompliance.getVchRoutineComplianceId();
        }
        catch (CustomGeneralException exception) {
            logger.error("Inside save method of RoutineComplianceServiceImpl some error occur:" + exception.getMessage());
            throw exception;
        }
        catch (Exception e) {
            logger.error("Inside save method of RoutineComplianceServiceImpl some error occur:" + e.getMessage());
            throw new CustomGeneralException(errorMessages.getUnknownError());
        }
    }

    @Override
    public RoutineComplianceDTO getById(String id) {
        logger.info("Inside getById method of RoutineComplianceServiceImpl");
        RoutineCompliance routineCompliance = routineComplianceRepository.findByVchRoutineComplianceIdAndBitDeleteFlag(id, false);

        return RoutineComplianceMapper.mapRoutineComplianceToDto(routineCompliance);
    }

    @Override
    public List<RoutineComplianceDTO> getAll() {
        logger.info("Inside getAll method of RoutineComplianceServiceImpl");
        List<RoutineCompliance> routineComplianceList = routineComplianceRepository.findAllByBitDeleteFlag(false);
        return routineComplianceList.stream()
                .map(RoutineComplianceMapper::mapRoutineComplianceToDto)
                .collect(Collectors.toList());
    }

    @Override
    public Page<RoutineComplianceDTO> getAll(Pageable pageable) {
        logger.info("Inside getAll pageable method of RoutineComplianceServiceImpl");
        Page<RoutineCompliance> routineCompliancePage = routineComplianceRepository.findAllByBitDeleteFlag(false, pageable);

        List<RoutineComplianceDTO> routineComplianceDTOList = routineCompliancePage.getContent().stream()
                .map(RoutineComplianceMapper::mapRoutineComplianceToDto)
                .collect(Collectors.toList());

        return new PageImpl<>(routineComplianceDTOList, pageable, routineCompliancePage.getTotalElements());
    }

    @Override
    public String deleteById(String id) {
        logger.info("Inside deleteById method of RoutineComplianceServiceImpl");
        RoutineCompliance routineCompliance = routineComplianceRepository.findByVchRoutineComplianceIdAndBitDeleteFlag(id, false);
        routineCompliance.setBitDeleteFlag(true);
        routineComplianceRepository.save(routineCompliance);
        return routineCompliance.getVchRoutineComplianceId();
    }

    @Override
    public Page<RoutineComplianceDTO> findByFilters(Integer roleId, Date fromDate, Date toDate, String search, String sortColumn, String sortDirection, Integer userId, Integer action, Pageable pageable) {
        logger.info("Inside findByFilters method of RoutineComplianceServiceImpl");

        Sort sort = Sort.by(Sort.Direction.fromString(sortDirection != null ? sortDirection : "DESC"),
                sortColumn != null ? sortColumn : "stmUpdatedAt");
        Pageable sortedPageable = PageRequest.of(pageable.getPageNumber(), pageable.getPageSize(), sort);

        List<Integer> myStages = getStagesByUserId(roleId, 0);

        Page<RoutineCompliance> routineCompliancePage = routineComplianceRepository.findByFilters(fromDate, toDate, search, userId, myStages.get(0), roleId, userIdConstants.getOicCompliance(), sortedPageable);

        return routineCompliancePage.map(RoutineComplianceMapper::mapRoutineComplianceToDto);
    }

    @Override
    public String takeAction(String data) {
        logger.info("Inside takeAction method of RoutineComplianceServiceImpl");

        String decodedData = CommonUtil.inputStreamDecoder(data);
        TakeActionRequest<RoutineCompliance> rcTakeAction;

        try {
            rcTakeAction = new ObjectMapper().readValue(decodedData, new TypeReference<>() {});
        } catch (Exception e) {
            logger.error("Inside save method of RoutineComplianceServiceImpl some error occur:" + e.getMessage());
            throw new CustomGeneralException(errorMessages.getInternalServerError());
        }

        String routineComplianceId = rcTakeAction.getData().getVchRoutineComplianceId();
        RoutineCompliance existingRoutineCompliance = routineComplianceRepository
                .findById(routineComplianceId)
                .orElseThrow(() -> new CustomGeneralException(errorMessages.getEntityNotFound()));

        Integer intCurrentRole = rcTakeAction.getOfficerRole();

        int approvalStage = existingRoutineCompliance.getIntCurrentStage();

        if (existingRoutineCompliance.getEnmStatus().equals(Status.OnHold) && Objects.equals(existingRoutineCompliance.getIntCurrentRole(), intCurrentRole)) {
            approvalStage += 1;
        }

        boolean isSecondCeo = false;

        try {
            isSecondCeo = Objects.equals(getNextRoutineOfficer(new CompositeKey(existingRoutineCompliance.getIntCurrentRole(), approvalStage)), userIdConstants.getCeo())
            && Objects.equals(existingRoutineCompliance.getIntCurrentRole(), userIdConstants.getCeo());
        }
        catch (CustomGeneralException e) {
            logger.info("Not final approval");
        }

        getApprovalDetails(existingRoutineCompliance, rcTakeAction, approvalStage);

        if (rcTakeAction.getAction().equals(Status.Approved)) {
            existingRoutineCompliance.setIntCurrentRole(getNextRoutineOfficer(new CompositeKey(intCurrentRole, approvalStage)));
            existingRoutineCompliance.setIntCurrentStage(approvalStage + 1);
            existingRoutineCompliance.setEnmStatus(Status.Pending);

            if (isSecondCeo) {
                existingRoutineCompliance.setEnmStatus(Status.Approved);
            }
        } else if (rcTakeAction.getAction().equals(Status.Deferred)) {
            getNextRoutineOfficer(new CompositeKey(intCurrentRole, existingRoutineCompliance.getIntCurrentStage()));

            existingRoutineCompliance.setIntCurrentRole(intCurrentRole);
            existingRoutineCompliance.setEnmStatus(Status.OnHold);
            existingRoutineCompliance.setIntCurrentStage(existingRoutineCompliance.getIntCurrentStage() - 1);
        }
        else if (rcTakeAction.getAction().equals(Status.Draft) || (rcTakeAction.getAction().equals(Status.Schedule) || rcTakeAction.getAction().equals(Status.Complete))&& Objects.equals(intCurrentRole, userIdConstants.getInspector())) {
            logger.info("Inspection draft, no action required.");
        }
        else {
            throw new CustomGeneralException(errorMessages.getNotAuthorized());
        }

        routineComplianceRepository.save(existingRoutineCompliance);
        return routineComplianceId;
    }

    private void getApprovalDetails(RoutineCompliance routineCompliance, TakeActionRequest<RoutineCompliance> rcTakeAction, int approvalStage) {
        int roleId = rcTakeAction.getOfficerRole();
        RoutineCompliance submittedRoutineCompliance = rcTakeAction.getData();

        // Map role IDs to their corresponding actions using Runnable instances (lambda expressions)
        Map<Integer, Runnable> approvalActions = Map.of(
                userIdConstants.getCeo(), () -> {
                    if (approvalStage == 1) {
                        if (submittedRoutineCompliance.getCeoApproval() == null) {
                            throw new CustomGeneralException("Ceo approval details required.");
                        }

                        routineCompliance.setCeoApproval(submittedRoutineCompliance.getCeoApproval());
                    }

                    if (approvalStage == 4) {
                        if (submittedRoutineCompliance.getCeoSecond() == null) {
                            throw new CustomGeneralException("Ceo Second approval details required.");
                        }
                        routineCompliance.setCeoSecond(submittedRoutineCompliance.getCeoSecond());
                    }
                },
                userIdConstants.getInspector(), () -> {

                    if (!Objects.equals(routineCompliance.getLeadInspector().getIntId(), rcTakeAction.getUserId())) {
                        throw new CustomGeneralException(errorMessages.getNotAuthorized());
                    }

                    if (submittedRoutineCompliance.getInspectorTwo() == null) {
                        throw new CustomGeneralException("Inspector two approval details required.");
                    }

                    RoutineComplianceInspectorTwo inspectorTwo = submittedRoutineCompliance.getInspectorTwo();

                    if (!(rcTakeAction.getAction().equals(Status.Schedule) || rcTakeAction.getAction().equals(Status.Complete))) {
                        inspectorTwo.setVchReportFilePath(JsonFileExtractorUtil.uploadFile(inspectorTwo.getVchReportFilePath(), "RC_InspectorTwo_Report_"+routineCompliance.getVchRoutineComplianceId(), FolderAndDirectoryConstant.ROUTINE_COMPLIANCE_INSPECTOR_TWO_FOLDER));
                        inspectorTwo.setVchPhotographicEvidenceFilePath(JsonFileExtractorUtil.uploadFile(inspectorTwo.getVchPhotographicEvidenceFilePath(), "RC_InspectorTwo_PhotographicEvidence_"+routineCompliance.getVchRoutineComplianceId(), FolderAndDirectoryConstant.ROUTINE_COMPLIANCE_INSPECTOR_TWO_FOLDER));
                    }

                    routineCompliance.setInspectorTwo(inspectorTwo);
                },
                userIdConstants.getOicCompliance(), () -> {

                    if (submittedRoutineCompliance.getComplianceTwo() == null) {
                        throw new CustomGeneralException("Compliance two approval details required.");
                    }

                    routineCompliance.setComplianceTwo(submittedRoutineCompliance.getComplianceTwo());
                }
        );

        Runnable action = approvalActions.get(roleId);
        if (action != null) {
            action.run();
        }
    }

    /**
     * Routine compliance Role Based
     */
    private final Map<CompositeKey, Integer> routineComplianceHierarchyMap = new HashMap<>();

    @PostConstruct
     public void setRoutineComplianceHierarchyMap() {
        routineComplianceHierarchyMap.put(new CompositeKey(userIdConstants.getOicCompliance(), 0), userIdConstants.getCeo());
        routineComplianceHierarchyMap.put(new CompositeKey(userIdConstants.getCeo(), 1), userIdConstants.getInspector());
        routineComplianceHierarchyMap.put(new CompositeKey(userIdConstants.getInspector(),2), userIdConstants.getOicCompliance());
        routineComplianceHierarchyMap.put(new CompositeKey(userIdConstants.getOicCompliance(), 3), userIdConstants.getCeo());
        routineComplianceHierarchyMap.put(new CompositeKey(userIdConstants.getCeo(), 4), userIdConstants.getCeo());
    }

    public int getNextRoutineOfficer(CompositeKey currentOfficer) {
        if (routineComplianceHierarchyMap.containsKey(currentOfficer)) {
            return routineComplianceHierarchyMap.get(currentOfficer);
        } else {
            throw new CustomGeneralException(errorMessages.getNotAuthorized());
        }
    }

    public List<Integer> getStagesByUserId(Integer userId, int direction) {
        List<Integer> stages = new ArrayList<>();

        for (CompositeKey key : routineComplianceHierarchyMap.keySet()) {
            if (key.getUserId().equals(userId)) {
                stages.add(key.getHierarchyLevel() + direction);
            }
        }

        return stages;
    }
}
